import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { combineLatest, map, Observable } from 'rxjs';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { FuelSalesService } from '../../services/fuel-sales.service';
import { FuelSalesFilter, FuelSalesResponse, FUEL_SALES_COLUMNS, FUEL_SALES_FILTER } from '../../models';

import { Column, ColumnDataType, SortType, tableDataSource } from'@sct-shared-lib';
import { ControlType, formControlBase } from '@sct-shared-lib';import { PaginationEvent }from '@sct-shared-lib';
import { CRUD_MODE, PATH, TABLE_NAME } from '@sct-shared-lib';
import { DATE_FORMAT, LOCALE_FORMAT } from '@sct-shared-lib';
import { PageableRequest } from '@sct-shared-lib';

@Component({
  selector: 'sct-fuel-sales-list',
  templateUrl: './fuel-sales-list.component.html',
  styleUrls: ['./fuel-sales-list.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuelSalesListComponent implements OnInit {
  public pageSize = 5;
  public totalElements = 0;
  public page = 1;

  filterValueChanged: any = {
    startDate: '',
    endDate: '',
    fuelType: '',
    productionPoint: '',
  };

  tableName = TABLE_NAME.FUEL_SALES;
  columns: Column[] = [];
  tableDataSource: tableDataSource[] = [];

  filterHasDateFilter = true;
  filterDataSource!: formControlBase<string>[];
  filterGroupData$!: Observable<formControlBase<string>[]>;
  refineryOptions$!: Observable<formControlBase<string>>;

  filterRefinery: FormControl = new FormControl('');

  constructor(private _fuelSalesService: FuelSalesService, private _router: Router, private _activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.columns = [...FUEL_SALES_COLUMNS];
    this._initalFilters();
    this._getDataTable();
    this.filterRefinery.valueChanges.subscribe((refineryValue) => {
      this._getDataTable();
    });
  }
  onView(fuelSale: any) {
    this._fuelSalesService.setFuelSales(fuelSale);
    const { id } = fuelSale;
    this._router.navigate([`${PATH.FUEL_SALES_UPDATE.route}`], {
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
      map(({ fuelType }) => {
        const startDateControl = { filterInfo: FUEL_SALES_FILTER.START_DATE, options: [] };
        const endDateControl = { filterInfo: FUEL_SALES_FILTER.END_DATE, options: [] };
        const fuelTypeControl = { filterInfo: FUEL_SALES_FILTER.FUEL_TYPE, options: fuelType };
        return [startDateControl, endDateControl, fuelTypeControl];
      })
    );
    this.filterGroupData$ = filterGroupData$.pipe(
      map((filterControl) => {
        return filterControl.map((filter) => this._formatFilterData(filter.filterInfo, filter.options));
      })
    );

    this.refineryOptions$ = filterData$.pipe(
      map(({ refineryState }) => {
        return this._formatFilterData(FUEL_SALES_FILTER.REFINERY_STATE, refineryState);
      })
    );
  }

  private _getDataFilter() {
    return combineLatest({
      refineryState: this._fuelSalesService.getRefineryState(),
      fuelType: this._fuelSalesService.getFuelType(),
    }).pipe(
      map(({ refineryState, fuelType }) => {
        return {
          refineryState: refineryState.content,
          fuelType: fuelType.content,
        };
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
    const { startDate, endDate, fuelType, productionPoint } = this.filterValueChanged;
    const payLoad: FuelSalesFilter = {
      startDate,
      endDate,
      refinery,
      fuelType,
    };
    this._fuelSalesService.getFuelSaleList(pageable, payLoad).subscribe((res) => {
      const { content, totalElements } = res;
      this.totalElements = totalElements;
      this.tableDataSource = this._formatDataTable(content);
    });
  }

  private _formatDataTable(data: FuelSalesResponse[]): tableDataSource[] {
    return data.map((res) => {
      return {
        rowData: { ...res },
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
}
