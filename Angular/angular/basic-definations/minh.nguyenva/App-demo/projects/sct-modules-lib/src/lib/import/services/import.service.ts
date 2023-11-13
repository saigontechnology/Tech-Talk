import { Injectable } from '@angular/core';
import { of, Observable ,BehaviorSubject} from 'rxjs';
import { GIM_REFINERY } from './../model/import.model';
import { COUNTRY_OF_ORIGIN, FUEL_TYPE, ImportFilter, ImportListRespone, IMPORT_LIST, IMPORT_POINT } from '../model';

import { Page, PageableRequest } from '@sct-shared-lib';

@Injectable({
  providedIn: 'root',
})
export class ImportService {
  private import$ = new BehaviorSubject<any>({});
  selectedImport$ = this.import$.asObservable();
  constructor() {}

  getImportList(pageable: PageableRequest, importFilterPayload?: ImportFilter): Observable<Page<ImportListRespone>> {
    const { page, size, sort, term } = pageable;
    const { startDate , endDate ,fuelType,  refinery, countryOfOrigin, importPoint  } = importFilterPayload || {};
    const dataRefreshed = [...IMPORT_LIST].map((data) => ({ ...data })).slice((page - 1) * size, (page - 1) * size + size);

    return of({
      content: [...dataRefreshed],
    totalElements: IMPORT_LIST.length,
    } as Page<ImportListRespone>);
  }

  getFuelType(): Observable<Page<any>> {
    const fuelTypeData = FUEL_TYPE;
    return of({
      content: [...fuelTypeData],
    } as Page<any>);
  }

  getCountryofOrigin(): Observable<Page<any>> {
    const countryOfOriginData = COUNTRY_OF_ORIGIN;
    return of({
      content: [...countryOfOriginData],
    } as Page<any>);
  }

  getImportPoint(): Observable<Page<any>> {
    const importPointData = IMPORT_POINT;
    return of({
      content: [...importPointData],
    } as Page<any>);
  }

  getRefineryState(): Observable<Page<any>> {
    const refinaryStateData = GIM_REFINERY;
    return of({
      content: [...refinaryStateData],
    } as Page<any>);
  }

  setImport(importItem: any){
    this.import$.next(importItem)
  }
}
