import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { combineLatest, map, Observable, Subject, takeUntil, tap } from 'rxjs';
import { compact, uniq } from 'lodash-es';

import { ProductionService } from '../../services/production.service';
import { ProductionPayLoad, ProductionListResponse, PRODUCTION_COLUMNS, PRODUCTION_FILTER, ProductionFilterValue } from '../../model';

import { CoreService } from '@core/services';

import {
  Column,
  ColumnDataType,
  LoaderService,
  tableDataSource,
  ControlType,
  formControlBase,
  PaginationEvent,
  CRUD_MODE,
  PATH,
  TABLE_NAME,
  DATE_FORMAT,
  LOCALE_FORMAT,
  PageableRequest,
  DateValidator,
  SITE_TYPE,
  LocationSiteResponse,
  FormControlService,
  NavigationService
} from '@sct-shared-lib';

@Component({
  selector: 'sct-production-list',
  templateUrl: './production-list.component.html',
  styleUrls: ['./production-list.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductionListComponent implements OnInit {
  PATH = PATH;
  public pageSize!: number;
  public totalElements!: number;
  public page!: number;
  public sortParams: string[] = ['lastModifiedTime,desc'];

  siteId = '';

  filterValueChanged: ProductionFilterValue = {};

  tableName = TABLE_NAME.PRODUCTION;
  columns: Column[] = [];
  tableDataSource: tableDataSource[] = [];

  filterDataSource!: formControlBase<string>[];
  filterGroupData$!: Observable<formControlBase<string>[]>;
  locationSiteOptions$!: Observable<formControlBase<string>>;

  filterLocationSite: FormControl = new FormControl('');

  private readonly _destroy$ = new Subject<void>();

  constructor(
    private _productionService: ProductionService,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _coreService: CoreService,
    private _cd: ChangeDetectorRef,
    private _loaderService: LoaderService,
    private _formControlService: FormControlService,
    private _navigationService: NavigationService,
  ) {}

  ngOnInit(): void {
    this.columns = [...PRODUCTION_COLUMNS];
    this._getLocationSite();
    this.filterLocationSite.valueChanges.subscribe((locationSiteValue: string) => {
      this.siteId = locationSiteValue;
      this._initFilterParams();

      this._getDataTable();
      this._initalFilters();
    });
  }

  ngOnDestroy() {
    this._destroy$.next();
    this._destroy$.unsubscribe();
  }

  onView(productionItem: ProductionListResponse) {
    this._productionService.setProduction({ selectedProduction: productionItem, siteId: this.siteId });
    const { id } = productionItem;
    this._router.navigate([`${PATH.PRODUCTION_UPDATE.route}`], {
      relativeTo: this._activatedRoute,
      queryParams: { id: id, mode: CRUD_MODE.UPDATE },
    });
  }

  onPaginationChange(event: PaginationEvent) {
    const { page, pageSize } = event;
    this.pageSize = pageSize;
    this.page = page;
    if (this.siteId) {
      this._getDataTable();
    }
  }

  onSort(sortParams: string[]) {
    const rawParams = [...sortParams, 'lastModifiedTime,desc'];
    this.sortParams = compact(rawParams);
    this._getDataTable();
  }

  getfilterFormGroupValue(filterGroupValue: ProductionFilterValue) {
    this.filterValueChanged = filterGroupValue;

    let startDate = this.filterValueChanged.startDate;
    let endDate = this.filterValueChanged.endDate;

    this.filterValueChanged.startDate = startDate ? new Date(startDate.year, startDate.month, startDate.day).toISOString() : '';
    this.filterValueChanged.endDate = endDate ? new Date(endDate.year, endDate.month, endDate.day).toISOString() : '';

    this._getDataTable();
  }

  private _initFilterParams() {
    const { startDate, endDate, fuelType, sort, page, size } = this._activatedRoute.snapshot.queryParams;
    let sortParams = sort;
    if (!!sort) {
      sortParams = [...this.sortParams];
    }

    this.pageSize = parseInt(size) || 5;
    this.page = !page || isNaN(page) ? 0 : (page || 0) * size;
    if (sortParams instanceof Array) {
      this.sortParams = sortParams;
    } else {
      this.sortParams = [sort || 'lastModifiedTime,desc'];
    }

    this.filterValueChanged.startDate = startDate;
    this.filterValueChanged.endDate = endDate;
    this.filterValueChanged.fuelType = fuelType;
  }

  private _initalFilters() {
    const filterData$ = this._getDataFilter();
    const filterGroupData$ = filterData$.pipe(
      map(({ fuelType }) => {
        const startDateControl = { filterInfo: PRODUCTION_FILTER.START_DATE, validate: [DateValidator.FULL_DATE], options: [] };
        const endDateControl = { filterInfo: PRODUCTION_FILTER.END_DATE, validate: [DateValidator.FULL_DATE], options: [] };
        const fuelTypeControl = { filterInfo: PRODUCTION_FILTER.FUEL_TYPE, validate: [], options: fuelType };
        return [startDateControl, endDateControl, fuelTypeControl];
      })
    );
    this.filterGroupData$ = filterGroupData$.pipe(
      map((filterControl) => {
        return filterControl.map((filter) => this._formControlService.formatFilterData(filter.filterInfo, filter.options, filter.validate));
      })
    );
  }

  private _getDataFilter() {
    return combineLatest({
      fuelType: this._coreService.fuelTypes$,
    }).pipe(
      map(({ fuelType }) => {
        return {
          fuelType: fuelType,
        };
      })
    );
  }

  private _getLocationSite() {
    this.locationSiteOptions$ = this._coreService.locationSite$.pipe(
      takeUntil(this._destroy$),
      tap((locationSite) => {}),
      map((locationSite) => {
        const locationFilterd = locationSite.filter((location: LocationSiteResponse) => location.typeCode === SITE_TYPE.REFINERIES.typeCode);
        this.filterLocationSite.patchValue(locationFilterd[0].id);
        return this._formControlService.formatFilterData(PRODUCTION_FILTER.LOCATION_SITE, locationFilterd);
      })
    );
  }

  private _getDataTable() {
    this._loaderService.showLoader();
    const page = this.page > 0 ? this.page - 1 : this.page;
    const size = this.pageSize;
    const sort = uniq([...this.sortParams]);
    const pageable: PageableRequest = {
      page,
      size,
      sort,
    };
    const siteId = this.filterLocationSite.getRawValue();
    const { startDate, endDate, fuelType } = this.filterValueChanged;
    const payLoad: ProductionPayLoad = {
      startDate,
      endDate,
      fuelType,
    };
    const queryParams: Params = {
      startDate,
      endDate,
      siteId,
      fuelType,
      sort,
      page,
      size: this.pageSize,
    };
    this._navigationService.updateRoute(queryParams);

    this._productionService.getProductionList(pageable, {}, this.siteId).subscribe((res) => {
      this._loaderService.hideLoader();
      const { content, totalPages } = res;
      this.totalElements = totalPages;
      this.tableDataSource = this._formatDataTable(content);

      this._cd.markForCheck();
      this._cd.detectChanges();
    });
  }

  private _formatDataTable(data: ProductionListResponse[]): tableDataSource[] {
    return data.map((res) => {
      return {
        rowData: { ...res },
        date: {
          data: res.requestedDate,
          type: ColumnDataType.STRING,
          pipeFormat: ['date', DATE_FORMAT.MOMENT_DATE_FORMAT, LOCALE_FORMAT.EN_USA],
        },
        fuelType: {
          data: res.fuelType.name,
          type: ColumnDataType.STRING,
        },
        volume: {
          data: res.volume.value,
          type: ColumnDataType.STRING,
          pipeFormat: ['unit'],
        },
      } as any;
    });
  }
}
