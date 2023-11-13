import { FormControl } from '@angular/forms';
import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { combineLatest, map, Observable } from 'rxjs';

import { ExportFilter, ExportListRespone, EXPORT_COLUMNS, EXPORT_FILTER } from '../../models';
import { ExportService } from '../../services/export.service';

import { Column, ColumnDataType, SortType, tableDataSource } from'@sct-shared-lib';
import { ControlType, formControlBase } from '@sct-shared-lib';import { PaginationEvent }from '@sct-shared-lib';
import { CRUD_MODE, PATH, TABLE_NAME } from '@sct-shared-lib';
import { DATE_FORMAT, LOCALE_FORMAT } from '@sct-shared-lib';
import { PageableRequest } from '@sct-shared-lib';


@Component({
  selector: 'sct-export-list',
  templateUrl: './export-list.component.html',
  styleUrls: ['./export-list.component.less'],
  encapsulation: ViewEncapsulation.Emulated,

  // changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExportListComponent implements OnInit {
  PATH = PATH;
  public pageSize = 5;
  public totalElements = 0;
  public page = 1;

  filterValueChanged: any = {
    startDate: '',
    endDate: '',
    fuelType: '',
    country: '',
    pointofOrigin: '',
  };

  tableName = TABLE_NAME.EXPORT;
  columns: Column[] = [];
  tableDataSource: tableDataSource[] = [];

  filterDataSource!: formControlBase<string>[];
  filterGroupData$!: Observable<formControlBase<string>[]>;
  refineryOptions$!: Observable<formControlBase<string>>;

  filterRefinery: FormControl = new FormControl('');

  constructor(private _exportService: ExportService,private _activatedRoute: ActivatedRoute, private _router: Router) {}

  ngOnInit(): void {
    this.columns = [...EXPORT_COLUMNS];
    this._initalFilters();
    this._getDataTable();
  }

  onView(exportItem: any) {
    this._exportService.setExport(exportItem);
    const { id } = exportItem;
    this._router.navigate([`${PATH.EXPORT_UPDATE.route}`], {
      relativeTo: this._activatedRoute,
      queryParams: { id: id, mode: CRUD_MODE.UPDATE},
    });
  }

  onPaginationChange(event: PaginationEvent) {
    const { page, pageSize } = event;
    this.pageSize = pageSize;
    this.page = page;
    this._getDataTable();
  }

  getfilterFormGroupValue(filterGroupValue: any) {
    this.filterValueChanged = filterGroupValue;
    this._getDataTable();
  }

  private _initalFilters() {
    const filterData$ = this._getDataFilter();
    const filterGroupData$ = filterData$.pipe(
      map(({ fuelType, countryOfOrigin, pointOrigin }) => {
        const startDateControl = { filterInfo: EXPORT_FILTER.START_DATE, options: [] };
        const endDateControl = { filterInfo: EXPORT_FILTER.END_DATE, options: [] };
        const fuelTypeControl = { filterInfo: EXPORT_FILTER.FUEL_TYPE, options: fuelType };
        const countryControl = { filterInfo: EXPORT_FILTER.COUNTRY, options: countryOfOrigin };
        const pointOfOrigin = { filterInfo: EXPORT_FILTER.POINT_OF_ORIGIN, options: pointOrigin };
        return [startDateControl, endDateControl, fuelTypeControl, countryControl, pointOfOrigin];
      })
    );
    this.filterGroupData$ = filterGroupData$.pipe(
      map((filterControl) => {
        return filterControl.map((filter) => this._formatFilterData(filter.filterInfo, filter.options));
      })
    );

    this.refineryOptions$ = filterData$.pipe(
      map(({ refineryState }) => {
        return this._formatFilterData(EXPORT_FILTER.REFINERY_STATE, refineryState);
      })
    );
  }

  private _getDataTable() {
    const page = this.page;
    const size = this.pageSize;
    const pageable: PageableRequest = {
      page,
      size,
    };
    const refinery = this.filterRefinery.getRawValue();
    const { startDate, endDate, fuelType, country, pointOfOrigin } = this.filterValueChanged;
    const payLoad: ExportFilter = {
      startDate,
      endDate,
      refinery,
      fuelType,
      country,
      pointOfOrigin,
    };
    const payload = {} as any;
    this._exportService.getExportList(pageable, payload).subscribe((res) => {
      const { content, totalElements } = res;
      this.totalElements = totalElements;
      this.tableDataSource = this._formatDataTable(content);
    });
  }
  private _getDataFilter() {
    return combineLatest({
      refineryState: this._exportService.getRefineryState(),
      fuelType: this._exportService.getFuelType(),
      countryOfOrigin: this._exportService.getCountryofOrigin(),
      pointOrigin: this._exportService.getPointOfOrigin(),
    }).pipe(
      map(({ refineryState, fuelType, countryOfOrigin, pointOrigin }) => {
        return {
          refineryState: refineryState.content,
          fuelType: fuelType.content,
          countryOfOrigin: countryOfOrigin.content,
          pointOrigin: pointOrigin.content,
        };
      })
    );
  }

  private _formatDataTable(data: ExportListRespone[]): tableDataSource[] {
    return data.map((res) => {
      return {
        rowData: { ...res },
        pointOfOrigin: {
          data: res.pointOfOrigin,
          type: ColumnDataType.STRING,
        },
        exportCountry: {
          data: res.exportCountry,
          type: ColumnDataType.STRING,
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
          pipeFormat :['unit']
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
}
