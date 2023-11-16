import { Component, AfterViewInit, Output, Input, EventEmitter, OnChanges, ViewChild, ElementRef } from '@angular/core';

import Map from 'ol/Map';
import Tile from 'ol/layer/Tile';
import { Point, Circle, Geometry } from 'ol/geom';
import { fromLonLat, toLonLat } from 'ol/proj';
import { Style, Icon, Stroke, Fill } from 'ol/style';
import VectorSource from 'ol/source/Vector';
import VectorLayer from 'ol/layer/Vector';
import { Zoom, Attribution, ScaleLine } from 'ol/control';
import OSM from 'ol/source/OSM';
import { Feature, View, MapBrowserEvent } from 'ol';


import { Coordinates } from '../site';
import { LOCATION_TYPE_TO_MARKER_MAP,MARKER_URLS } from './../../../constant-variable';

@Component({
  selector: 'sct-map',
  templateUrl: './map.component.html'
})
export class MapComponent implements AfterViewInit, OnChanges {
  @ViewChild('map', {static: true}) mapRef!: ElementRef;
  // parameters to calculate the radius for circle
  static readonly earthRadius = 6367.5 * 1000.0;
  static readonly DEG_2_RAD = Math.PI / 180.0;
  static readonly RAD_2_DEG = 180.0 / Math.PI;

  @Input() inputCoordinates: any  = {
    latitude: -1.266667,
    longitude: 36.8
  };
  @Input() mapDisabled = false;
  @Input() locationCoordinates!: Coordinates[];
  @Input() centerCoordinate!: Coordinates;
  @Input() distanceFromCenter!: number;
  @Input() zoomLevel = 12;
  @Input() mapId = 'map';
  @Input() selectMode: boolean = false;
  @Input() title = '';

  @Output() selectedCoordinate: EventEmitter<Coordinates> = new EventEmitter<Coordinates>();

  // openlayer variables
  map!: Map;
  markerStyle!: Style;
  circle!: Style;
  pointer!: Feature<Point>;
  circleFeature!: Feature<Circle>;
  vectorSource!: VectorSource<Geometry>;
  vectorLayer!: VectorLayer<any>;
  view!: View;

  clickedCoordinates!: Coordinates;
  circleDistance!: number;

  constructor() { }

  ngAfterViewInit() {
    this.initMapFeatures();
  }

  ngOnChanges() {
    if (this.centerCoordinate && this.map) {
      this.setMapMarker(this.centerCoordinate.longitude, this.centerCoordinate.latitude);
      this.setCircleFeature();
      if (this.locationCoordinates) {
        this.createOtherLocationsMarkers();
      }
    }
  }

  initMapFeatures() {

    // if the input coordinates are empty will be replaced by manila coordinates
    if (!this.centerCoordinate) {
      const { longitude, latitude } = this.inputCoordinates;
      this.centerCoordinate = {
        longitude,
        latitude
      };
    }

    this.vectorSource = new VectorSource();

    this.setCircleFeature();
    // Condition not to create center marker if no center coordinate is input
    if (this.centerCoordinate) {
      // Creating center marker
      this.markerStyle = new Style({
        image: new Icon(({
          src: MARKER_URLS['MARKER_DEFAULT'],
          imgSize: [250, 250],
          scale: 0.25
        }))
      });

      // Creating the center point
      this.pointer = new Feature(new Point(fromLonLat([this.centerCoordinate.longitude, this.centerCoordinate.latitude])));

      // Add the merker to the center point and add as a feature of the map
      this.pointer.setStyle(this.markerStyle);
      this.vectorSource.addFeature(this.pointer);
    }

    this.createOtherLocationsMarkers();
    // creating vector layer
    this.vectorLayer = new VectorLayer({
      source: this.vectorSource,
    });

    // creating the view
    this.view = new View({
      center: fromLonLat([this.centerCoordinate.longitude, this.centerCoordinate.latitude]),
      zoom: this.zoomLevel,
    });

    // creating the map
    this.map = new Map({
      target: this.mapRef.nativeElement,
      layers: [
        new Tile({
          source: new OSM(),
        }),
        this.vectorLayer
      ],
      controls: [
        new Zoom(),
        new Attribution(),
        new ScaleLine(),
      ],
      view: this.view
    });

    if (!this.mapDisabled) {
      this.onMapClick();
    }
  }

  setMapMarker(lon: number, lat: number) {
    const formattedLonLat = fromLonLat([lon, lat]) || undefined;

    if (!formattedLonLat) {
      return;
    }

    this.pointer.setGeometry(new Point(formattedLonLat));
    this.pointer.setStyle(this.markerStyle);

    this.vectorSource.clear(true);
    this.vectorSource.addFeature(this.pointer);

    this.view.animate({
      center: formattedLonLat,
      duration: 2000
    });
  }

  setMarkerStyle(location: Coordinates): Style {
    return new Style({
      image: new Icon(({
        src: LOCATION_TYPE_TO_MARKER_MAP[location.locationType as keyof typeof LOCATION_TYPE_TO_MARKER_MAP],
        imgSize: [250, 250],
        scale: 0.25
      }))
    });
  }

  createOtherLocationsMarkers() {

    // Condition not to create other markers if not needed
    if (this.locationCoordinates) {

      // adding a point for each coordinate present in the list
      this.locationCoordinates.forEach((locationCoordinate, index) => {
        this.pointer = new Feature(new Point(fromLonLat([
          locationCoordinate.longitude,
          locationCoordinate.latitude,
        ])));
        // an id is added to each point to retrieve back the value later on
        this.pointer.setId(index);
        this.pointer.setStyle(this.setMarkerStyle(locationCoordinate));
        this.vectorSource.addFeature(this.pointer);
      });
    }
  }

  setCircleFeature() {
    // Condition not to create center circle if no input
    if (this.distanceFromCenter) {
      // calculate circle radius
      this.circleDistance = this.distanceFromCenter * 1000;
      const deltaAngle = (this.circleDistance / MapComponent.earthRadius) * MapComponent.RAD_2_DEG;
      const p1XY = fromLonLat([this.centerCoordinate.longitude, this.centerCoordinate.latitude]);
      const p2XY = fromLonLat([this.centerCoordinate.longitude + deltaAngle, this.centerCoordinate.latitude]);
      const distancePixel = Math.sqrt((p1XY[0] - p2XY[0]) * (p1XY[0] - p2XY[0]) + (p1XY[1] - p2XY[1]) * (p1XY[1] - p2XY[1]));

      // Creating circle style
      this.circle = new Style({
        fill: new Fill({
          color: 'rgba(0,255,255,0.3)',
        }),
        stroke: new Stroke({
          color: 'rgba(0,255,255,0.6)',
          width: 3
        })
      });
      // Creating circle feature
      this.circleFeature = new Feature(new Circle(fromLonLat([this.centerCoordinate.longitude, this.centerCoordinate.latitude]),
        distancePixel));

      this.circleFeature.setStyle(this.circle);
      this.vectorSource.addFeature(this.circleFeature);
    }
  }

  onMapClick() {

    this.map.on('click', (map: MapBrowserEvent<any>) => {
      const lonlat = toLonLat(map.coordinate);
      const lon = lonlat[0];
      const lat = lonlat[1];

      this.markerStyle = new Style({
        image: new Icon(({
          src: MARKER_URLS['MARKER_DEFAULT'],
          imgSize: [250, 250],
          scale: 0.25
        }))
      });

      this.pointer.setStyle(this.markerStyle);

      if (this.map.hasFeatureAtPixel(map.pixel) && this.locationCoordinates) {
        const coordinate = map.map.getFeaturesAtPixel(map.pixel);
        const id = coordinate[0].getId();
        if (id as number >= 0) {
          this.selectedCoordinate.emit(this.locationCoordinates[id as number]);
          this.setMapMarker(lon, lat);
        }
      } else {
        if (!this.selectMode) {
          this.setMapMarker(lon, lat);
          this.selectedCoordinate.emit({
            longitude: lon,
            latitude: lat
          });
        }
      }
    });
  }
}
