import {
  Component,
  Output,
  Input,
  EventEmitter,
  ViewChild,
  ElementRef,
  AfterViewInit,
  NgZone,
  OnDestroy,
  ChangeDetectorRef,
  OnInit,
} from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

import { Feature, View, MapBrowserEvent } from 'ol';
import Map from 'ol/Map';
import { Point, Circle } from 'ol/geom';
import { fromLonLat, toLonLat } from 'ol/proj';
import { Style, Icon, Stroke, Fill } from 'ol/style';
import { Zoom, Attribution, ScaleLine } from 'ol/control';
import VectorSource from 'ol/source/Vector';
import VectorLayer from 'ol/layer/Vector';
import OSM from 'ol/source/OSM';
import TileLayer from 'ol/layer/Tile';
import { Coordinate, format } from 'ol/coordinate';

import { LOCATION_TYPE_TO_MARKER_MAP, MAP_LAYERS, MARKER_URLS } from './../../../constant-variable';
import { UtilitiesService } from './../../../service/utilities.service';

import { Coordinates } from '../site';
import { MAP_CONFIG } from '../constant';
import { MapMode } from '../enums';
import { FeatureStyle, MapEvent } from '../interface';



@Component({
  selector: 'sct-simple-map',
  templateUrl: './simple-map.component.html',
  styleUrls: ['./simple-map.component.less'],
})
export class SimpleMapComponent implements AfterViewInit, OnDestroy {
  @ViewChild('map', { static: true }) mapRef!: ElementRef;

  @Input() inputCoordinates: any  = {
    latitude: 0,
    longitude: 0
  };

  @Input() searchable: boolean = false;
  @Input() disabled: boolean = false;
  @Input() showLatlngText: boolean = true;

  @Input() title = '';
  @Input() mapId = 'map';
  @Input() zoomLevel = MAP_CONFIG.defaultZoom;
  @Input() features!: Coordinates[];
  @Input() distanceFromCenter!: number;
  @Input() modes: MapMode[] = [MapMode.MOVE_CENTER];
  @Input() delay = 0;
  @Input('center') set center(value: Coordinates) {
    if (!value) {
      return;
    }

    const { longitude, latitude } = value;
    const [lng, lat] = fromLonLat([longitude, latitude]);
    this._center = { ...value, latitude: lat, longitude: lng };
    if (!this._map) {
      return;
    }
    this._setCenter(this._center, false);
  }

  get center() {
    return this._center;
  }

  @Output() onceInit: EventEmitter<void> = new EventEmitter<void>();
  @Output() selected: EventEmitter<MapEvent> = new EventEmitter<MapEvent>();
  @Output() centerChanged = new EventEmitter<MapEvent>();

  searchForm!: FormGroup;
  defaultCoordinates = [0,0];
  viewCenterLatLng = '';

  private _map!: Map;
  private _center!: Coordinates;
  private _featureLayer!: VectorLayer<VectorSource>;
  private _controlLayer!: VectorLayer<VectorSource>;
  private _features: Coordinates[] = [];
  private _timeout!: any;

  constructor(private _ngZone: NgZone, private _cd: ChangeDetectorRef, private _utilitiesService: UtilitiesService) {
    this.initFormSearch();
  }

  ngAfterViewInit(): void {
    this._timeout = setTimeout(() => {
      this.init();
      this._map.updateSize();
      this._timeout = null;
    }, this.delay);
  }

  ngOnDestroy(): void {
    if (this._timeout) {
      clearTimeout(this._timeout);
    }
    if (this._map) {
      this._map.dispose();
    }
  }

  public gotoCoordinate() {
    if (!this.searchForm.valid) {
      return;
    }
    const { coordinateText } = this.searchForm.getRawValue();

    const [longitude, latitude] = this.textToCoordinate(coordinateText);
    const [lng, lat] = fromLonLat([longitude, latitude]);
    this._setCenter({ latitude: lat, longitude: lng });
  }

  public clearForm(){
    this.searchForm.reset();
  }

  public init() {
    const defaultCoordinates = this.inputCoordinates;
    const { longitude, latitude } = this.inputCoordinates;
    this.defaultCoordinates = [longitude, latitude]

    const [lng, lat] = fromLonLat([longitude, latitude]);
    const _defaultCoordinates = { ...defaultCoordinates, latitude: lat, longitude: lng };
    const center = this.center ? this.center : { ..._defaultCoordinates };

    const zoomLevel = this.zoomLevel ? this.zoomLevel : MAP_CONFIG.defaultZoom;

    this._map = this._initMap(center, zoomLevel);

    if (!this._map) {
      return;
    }

    this._setCenter(center, false);

    this._addLayerToMap(MAP_LAYERS.FEATURES);

    this._onMapHover();
    this._onMapClick();
    this._onMapEnd();

    if (this.features) {
      this.setFeatures(this.features);
    }

    this.onceInit.emit();
  }

  public resetSource() {
    (this._featureLayer.getSource() as VectorSource).clear(true);
    this.features = [];
  }

  public setFeatures(features: Coordinates[]) {
    const markers = (features || []).map((feature, index) => {
      const { longitude, latitude } = feature;
      const [lng, lat] = fromLonLat([longitude, latitude]);
      return this._getMarkerFeature(index, { ...feature, latitude: lat, longitude: lng });
    });
    this._features = [...features];
    if (this._featureLayer) {
      const source = this._featureLayer.getSource() as VectorSource;
      source.clear(true);
      source.addFeatures(markers);
    }
  }

  public setDistanceFromCenter(distanceFromCenter: number) {
    this.distanceFromCenter = distanceFromCenter;
    this._setCenter(this.center, false);
  }

  public rerender() {
    if (!this._map) {
      return;
    }
    this._map.render();
  }

  private _setCenter(coordinate: Coordinates, isEmit = true, isZoomTo = true) {
    this._center = { ...coordinate };

    if (!this._controlLayer) {
      this._controlLayer = this._getVectorLayer(MAP_LAYERS.CONTROL);
      this._map.addLayer(this._controlLayer);
    }

    const centerFeature = this._getMarkerFeature(undefined, this.center);
    const source = this._controlLayer.getSource();
    if(source){ // added
      source.clear(true);
      source.addFeature(centerFeature);
  
      if (this.distanceFromCenter) {
        const circleFeature = this._getCircleFeature(this.center, this.distanceFromCenter);
        source.addFeature(circleFeature);
      }
  
      if (isZoomTo){
        const centerCoord =   (centerFeature.getGeometry() as any).getCoordinates()
        this._map.getView().animate({
          center: centerCoord,
          duration: 500
        });
      }
      
      if (isEmit) {
        const { longitude, latitude } = this.center;
        const [lng, lat] = toLonLat([longitude, latitude]);
        this.centerChanged.emit({ id: null, coordinates: { longitude: lng, latitude: lat } });
      }
    }
  }

  private _onMapEnd() {
    this._map.on('moveend' as any, (mapEvent: MapBrowserEvent<any>) => {
      const { map } = mapEvent;
      const center = map.getView().getCenter();
      const viewCenter = toLonLat(center as any);
      this.viewCenterLatLng = format(viewCenter, '{y}, {x}', 6)
      this._cd.detectChanges();
    });
  }

  private _onMapHover() {
    this._map.on('pointermove', (mapEvent: MapBrowserEvent<any>) => {
      if (this.disabled){
        return;
      }
      const hit = mapEvent.map.forEachFeatureAtPixel(mapEvent.pixel, (feature, layer) => {
        const layerId = layer.getProperties()['id'];
        if (layerId === MAP_LAYERS.FEATURES) {
          return true;
        }
        return false;
      });
      if (hit) {
        mapEvent.map.getTargetElement().style.cursor = 'pointer';
      } else {
        mapEvent.map.getTargetElement().style.cursor = '';
      }
    });
  }

  private _onMapClick() {
    this._map.on('click', (mapEvent: MapBrowserEvent<any>) => {
      if (this.disabled){
        return;
      }
      if (this.modes.includes(MapMode.SELECTION) && this._features?.length > 0) {
        this._clickOnFeature(mapEvent);
        return;
      }
      if (this.modes.includes(MapMode.MOVE_CENTER)) {
        this._clickToMoveCenter(mapEvent);
        return;
      }
    });
  }

  private _clickOnFeature(mapEvent: MapBrowserEvent<any>) {
    const { pixel, map } = mapEvent;
    const features = map.getFeaturesAtPixel(pixel);
    if (!(features && features.length > 0)) {
      return;
    }

    const firstFeature = features.find((e) => (this._featureLayer.getSource()as any).hasFeature(e as any));
    if (!firstFeature) {
      return;
    }

    const id = firstFeature.getId();
    this._setActiveMarker(id);
  }

  private _clickToMoveCenter(mapEvent: MapBrowserEvent<any>) {
    const {
      coordinate: [longitude, latitude],
    } = mapEvent;
    this._setCenter({ latitude, longitude });
  }

  private _addLayerToMap(id: MAP_LAYERS) {
    this._featureLayer = this._getVectorLayer(id);
    this._map.addLayer(this._featureLayer);
  }

  private _initMap(center: Coordinates, zoom: number): Map {
    return this._ngZone.runOutsideAngular(() => {
      // creating the view
      const { longitude, latitude } = center;
      const view = new View({
        center: [longitude, latitude],
        zoom,
      });

      const tileLayer = new TileLayer({
        source: new OSM(),
      });
      // creating the map
      return new Map({
        target: this.mapRef.nativeElement,
        layers: [tileLayer],
        controls: [new Zoom(), new Attribution(), new ScaleLine()],
        view,
      });

    });
  }

  private _getVectorLayer(id: string): VectorLayer<VectorSource> {
    const source = new VectorSource();
    return new VectorLayer({
      properties: {
        id,
      },
      source,
    });
  }

  /* Point */
  private _getMarkerFeature(id: any, center: Coordinates) {
    const { locationType, latitude, longitude } = center;
    const style = this._getMarkerStyle({ locationType });
    const feature = new Feature(new Point([longitude, latitude]));
    feature.setId(id);
    feature.setStyle(style);
    feature.setProperties({ _coordinates: { ...center, isSelected: false } }, true);
    return feature;
  }

  private _getMarkerStyle(opts: FeatureStyle): Style {
    const { locationType, isSelected } = opts;
    const src_marker_type = locationType && LOCATION_TYPE_TO_MARKER_MAP[locationType] ? LOCATION_TYPE_TO_MARKER_MAP[locationType] : 'MARKER_DEFAULT';
    const src_marker = isSelected ? `${src_marker_type}_SELECTED` : src_marker_type;
    const src = MARKER_URLS[src_marker as keyof typeof MARKER_URLS] ? MARKER_URLS[src_marker as keyof typeof MARKER_URLS] : MARKER_URLS['MARKER_DEFAULT'];
    const scale = isSelected ? MAP_CONFIG.activeScale : MAP_CONFIG.defaultScale;
    return new Style({
      image: new Icon({
        src,
        imgSize: [250, 250],
        scale,
      }),
    });
  }

  private _setActiveMarker(id: any) {
    (this._featureLayer.getSource() as VectorSource).forEachFeature(ft =>{
      const _coordinates = ft.getProperties()['_coordinates'];
      const { locationType } = _coordinates;
      if (id === ft.getId()) {
        const style = this._getMarkerStyle({ locationType, isSelected: true });
        ft.setStyle(style);
        const _updated_coordinates = { ..._coordinates, isSelected: true };
        ft.setProperties({ _coordinates: _updated_coordinates }, false);
        this.selected.emit({ id, coordinates: { ..._updated_coordinates } });
      } else {
        const style = this._getMarkerStyle({ locationType, isSelected: false });
        ft.setStyle(style);
      }
    })
  }

  /* End Point */

  /* Circle */
  private _getCircleFeature(center: Coordinates, distanceFromCenter: number) {
    const circleStyle = new Style({
      fill: new Fill({
        color: 'rgba(0,255,255,0.3)',
      }),
      stroke: new Stroke({
        color: 'rgba(0,255,255,0.6)',
        width: 3,
      }),
    });
    // Creating circle feature
    const { latitude, longitude } = center;
    const feature = new Feature({
      geometry: new Circle([longitude, latitude], distanceFromCenter * 1000),
    });

    feature.setStyle(circleStyle);
    return feature;
  }
  /* End Circle */

  private textToCoordinate(value: string): Coordinate | any {
    const [lat, lng] = (value as string).replace(' ', '').split(',');
    const coord = [Number.parseFloat(lng), Number.parseFloat(lat)];
    const isValid = this._utilitiesService.checkValidCoordinate(coord);
    return isValid ? coord : null;
  }

  private initFormSearch() {
    if (!this.searchForm) {
      const coordinateValidator = () => {
        return (control: AbstractControl): ValidationErrors | null => {
          const value = control.value;

          if (!value) {
            return null;
          }

          const coord = this.textToCoordinate(value);
          return !coord ? { coordinateInvalid: true } : null;
        };
      };
      this.searchForm = new FormGroup({
        coordinateText: new FormControl(null, [Validators.required, coordinateValidator()]),
      });
    }
  }
}
