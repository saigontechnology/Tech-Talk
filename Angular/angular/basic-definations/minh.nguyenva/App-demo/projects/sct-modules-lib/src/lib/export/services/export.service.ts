import { Injectable } from '@angular/core';
import { Observable, of ,BehaviorSubject} from 'rxjs';

import { EXPORT_COUNTRY, EXPORT_COUNTRY_OF_ORIGIN, ExportFilter, ExportListRespone, EXPORT_LIST, EXPORT_POINT_OF_ORIGIN } from '../models';
import { EXPORT_FUEL_TYPE, EXPORT_GIM_REFINERY } from '../models';

import { Page, PageableRequest } from '@sct-shared-lib';

@Injectable({
  providedIn: 'root'
})
export class ExportService {
  private export$ = new BehaviorSubject<any>({});
  selectedExport$ = this.export$.asObservable();
  constructor() { }

  getExportList(pageable: PageableRequest, importFilterPayload?: ExportFilter): Observable<Page<ExportListRespone>> {
    const { page, size, sort, term } = pageable;
    // const { startDate , endDate ,fuelType,  refinery, countryOfOrigin, importPoint  } = importFilterPayload || {};
    const dataRefreshed = [...EXPORT_LIST].map((data) => ({ ...data })).slice((page - 1) * size, (page - 1) * size + size);

    return of({
      content: [...dataRefreshed],
      totalElements: EXPORT_LIST.length,
    } as Page<ExportListRespone>);
  }
  getFuelType(): Observable<Page<any>> {
    const data = EXPORT_FUEL_TYPE;
    return of({
      content: [...data],
    } as Page<any>);
  }

  getCountry(): Observable<Page<any>> {
    const data = EXPORT_COUNTRY;
    return of({
      content: [...data],
    } as Page<any>);
  }

  getPointOfOrigin(): Observable<Page<any>> {
    const data = EXPORT_POINT_OF_ORIGIN;
    return of({
      content: [...data],
    } as Page<any>);
  }

  getRefineryState(): Observable<Page<any>> {
    const data = EXPORT_GIM_REFINERY;
    return of({
      content: [...data],
    } as Page<any>);
  }

  getCountryofOrigin(): Observable<Page<any>> {
    const countryOfOriginData = EXPORT_COUNTRY_OF_ORIGIN;
    return of({
      content: [...countryOfOriginData],
    } as Page<any>);
  }

  
  setExport(exportItem: any){
    this.export$.next(exportItem)
  }
}
