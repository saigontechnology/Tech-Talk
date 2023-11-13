import { NgStyle, NgClass } from '@angular/common';
import { ChangeDetectionStrategy, Component, ComponentFactoryResolver, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { combineLatest, map, Observable } from 'rxjs';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { compact, uniq } from 'lodash-es';

import { ImportService } from '../../services/import.service';
import { ImportFilter, ImportListRespone, IMPORT_COLUMNS, IMPORT_FILTER } from '../../model';

import { CircleProgressComponent, Column, ColumnDataType, LevelProgressComponent, SortType, tableDataSource } from '@sct-shared-lib';
import { ControlType, formControlBase } from '@sct-shared-lib';
import { PaginationEvent } from '@sct-shared-lib';
import { CRUD_MODE, PATH, TABLE_NAME } from '@sct-shared-lib';
import { DATE_FORMAT, LOCALE_FORMAT } from '@sct-shared-lib';
import { PageableRequest } from '@sct-shared-lib';

@Component({
  selector: 'sct-import-list',
  templateUrl: './import-list.component.html',
  styleUrls: ['./import-list.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ImportListComponent implements OnInit {
  @ViewChild('dynamicInfo', { read: ViewContainerRef, static: true })
  containerRef!: ViewContainerRef;

  PATH = PATH;
  public pageSize = 5;
  public totalElements = 0;
  public page = 1;
  public sortParams: string[] = ['jerricanIdCode,asc', 'lastModifiedTime,desc'];

  filterValueChanged: any = {
    startDate: '',
    endDate: '',
    fuelType: '',
    countryOfOrigin: '',
    importPoint: '',
  };

  tableName = TABLE_NAME.IMPORT;
  columns: Column[] = [];
  tableDataSource: tableDataSource[] = [];

  filterHasDateFilter = true;
  filterDataSource!: formControlBase<string>[];
  filterGroupData$!: Observable<formControlBase<string>[]>;
  refineryOptions$!: Observable<formControlBase<string>>;

  filterRefinery: FormControl = new FormControl('');

  constructor(
    private _importService: ImportService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private cfr: ComponentFactoryResolver
  ) {
    console.log(this.containerRef)
  }

  ngOnInit(): void {
    this.columns = [...IMPORT_COLUMNS];
    this._initalFilters();
    this.filterRefinery.valueChanges.subscribe((refineryValue) => {
      this._getDataTable();
    });
    this._initFilterParams();
  }

  importInfo() {
    this.containerRef.clear()
    // cfr: recieve a class component 4 load dynamic & create it's component factory. ViewcontainerRef user componentfactory 4 load dynamic components/ 
    const componentFactory = this.cfr.resolveComponentFactory(LevelProgressComponent); // class for create Component; a result of cfr.resolve...
    //add the component to the view
    const componentRef = this.containerRef.createComponent(componentFactory);

    //interact dynamic components (pass some date to component)
    componentRef.instance.progressName = 'Angular Rodmap';
    componentRef.instance.progressData = { maxVolume: 100,
      currentVolume : 20,
      percentVolume: 30,
      unitType : 'm³',
      progressName: 'Import Progress'}

  }


  clearimportInfo() {
    this.containerRef.clear();
  }

  async importInfoLazyLoad() {
    this.containerRef.clear()
    const { LevelProgressComponent } = await import('@sct-shared-lib');
    const componentFactory = this.cfr.resolveComponentFactory(
      LevelProgressComponent
    );
    const componentRef = this.containerRef.createComponent(componentFactory);

    //interact dynamic components
    componentRef.instance.progressName = 'Import Progress';
    componentRef.instance.progressData = { maxVolume: 100,
      currentVolume : 30,
      percentVolume: 30,
      unitType : 'm³',
      progressName: 'Import Progress'}

  }

  onView(importItem: any) {
    this._importService.setImport(importItem);
    const { id } = importItem;
    this._router.navigate([`${PATH.IMPORT_UPDATE.route}`], {
      relativeTo: this._activatedRoute,
      queryParams: { id: id, mode: CRUD_MODE.UPDATE },
    });
  }

  onPaginationChange(event: PaginationEvent) {
    const { page, pageSize } = event;
    this.pageSize = pageSize;
    this.page = page;
    this._getDataTable();
  }

  onSort(sortParams: string[]) {
    const rawParams = [...sortParams, 'lastModifiedTime,desc'];
    this.sortParams = compact(rawParams);
    this._getDataTable();
  }

  getfilterFormGroupValue(filterGroupValue: any) {
    this.filterValueChanged = filterGroupValue;
    this._getDataTable();
  }

  private _initFilterParams() {
    const { startDate, endDate, fuelType, countryOfOrigin, importPoint, sort, page, size } = this._activatedRoute.snapshot.queryParams;

    this.pageSize = parseInt(size) || 5;
    this.page = !page || isNaN(page) ? 0 : (page || 0) * size;
    if (sort instanceof Array) {
      this.sortParams = sort;
    } else {
      this.sortParams = [sort || 'lastModifiedTime,desc'];
    }

    this.filterValueChanged.startDate = startDate;
    this.filterValueChanged.endDate = endDate;
    this.filterValueChanged.fuelType = fuelType;
    this.filterValueChanged.countryOfOrigin = countryOfOrigin;
    this.filterValueChanged.importPoint = importPoint;

    this._getDataTable();
  }

  private _initalFilters() {
    const filterData$ = this._getDataFilter();
    const filterGroupData$ = filterData$.pipe(
      map(({ fuelType, countryOfOrigin, importPoint }) => {
        const startDateControl = { filterInfo: IMPORT_FILTER.START_DATE, options: [] };
        const endDateControl = { filterInfo: IMPORT_FILTER.END_DATE, options: [] };
        const fuelTypeControl = { filterInfo: IMPORT_FILTER.FUEL_TYPE, options: fuelType };
        const countryControl = { filterInfo: IMPORT_FILTER.COUNTRY_OF_ORIGIN, options: countryOfOrigin };
        const importPointControl = { filterInfo: IMPORT_FILTER.IMPORT_POINT, options: importPoint };
        return [startDateControl, endDateControl, fuelTypeControl, countryControl, importPointControl];
      })
    );
    this.filterGroupData$ = filterGroupData$.pipe(
      map((filterControl) => {
        return filterControl.map((filter) => this._formatFilterData(filter.filterInfo, filter.options));
      })
    );

    this.refineryOptions$ = filterData$.pipe(
      map(({ refineryState }) => {
        return this._formatFilterData(IMPORT_FILTER.REFINERY_STATE, refineryState);
      })
    );
  }

  private _getDataFilter() {
    return combineLatest({
      refineryState: this._importService.getRefineryState(),
      fuelType: this._importService.getFuelType(),
      countryOfOrigin: this._importService.getCountryofOrigin(),
      importPoint: this._importService.getImportPoint(),
    }).pipe(
      map(({ refineryState, fuelType, countryOfOrigin, importPoint }) => {
        return {
          refineryState: refineryState.content,
          fuelType: fuelType.content,
          countryOfOrigin: countryOfOrigin.content,
          importPoint: importPoint.content,
        };
      })
    );
  }

  private _getDataTable() {
    const page = this.page;
    const size = this.pageSize;
    const sort = uniq([...this.sortParams]);

    const pageable: PageableRequest = {
      page,
      size,
      sort,
    };
    const refinery = this.filterRefinery.getRawValue();
    const { startDate, endDate, fuelType, countryOfOrigin, importPoint } = this.filterValueChanged;
    const payLoad: ImportFilter = {
      startDate,
      endDate,
      refinery,
      fuelType,
      countryOfOrigin,
      importPoint,
    };

    const queryParams: Params = {
      startDate,
      endDate,
      refinery,
      fuelType,
      countryOfOrigin,
      importPoint,
      sort,
      page,
      size: this.pageSize,
    };
    this._updateRoute(queryParams);

    this._importService.getImportList(pageable, payLoad).subscribe((res) => {
      const { content, totalElements } = res;
      this.totalElements = totalElements;
      this.tableDataSource = this._formatDataTable(content);
    });
  }

  private _formatDataTable(data: ImportListRespone[]): tableDataSource[] {
    return data.map((res) => {
      return {
        rowData: { ...res },
        importPoint: {
          data: res.importPoint,
          type: ColumnDataType.STRING,
          sort: SortType.EMPTY,
        },
        countryOfOrigin: {
          data: res.countryOfOrigin.value,
          type: ColumnDataType.STRING,
          ngStyle: { ['font-weight']: 'bold', ['color']: 'green' },
        },
        date: {
          data: res.date,
          type: ColumnDataType.STRING,
          pipeFormat: ['date', DATE_FORMAT.MOMENT_DATE_FORMAT, LOCALE_FORMAT.EN_USA],
        },
        fuelType: {
          data: res.fuelType.value,
          type: ColumnDataType.STRING,
        },
        volume: {
          data: res.volume,
          type: ColumnDataType.STRING,
          pipeFormat: ['unit'],
        },
      } as any;
    });
  }

  private _formatFilterData(filterInfo: any, options: any[]): formControlBase<string> {
    return {
      key: filterInfo.name,
      label: filterInfo.displayName,
      controlType: filterInfo.controlType,
      options:
        filterInfo.controlType === ControlType.SELECT
          ? options.map((option) => {
              return {
                key: option.name,
                value: option.name,
                display: option.value,
              };
            })
          : [],
    };
  }

  private _updateRoute(queryParams: Params) {
    this._router.navigate([], {
      queryParams,
      relativeTo: this._activatedRoute,
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }
}
