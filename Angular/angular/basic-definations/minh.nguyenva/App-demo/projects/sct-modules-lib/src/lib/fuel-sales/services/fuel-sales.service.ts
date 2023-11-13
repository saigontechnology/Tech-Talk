import { Injectable } from '@angular/core';
import { Observable, of,BehaviorSubject } from 'rxjs';
import { FuelSalesFilter, FUEL_SALES_LIST, FuelSalesResponse, FUEL_SALES_GIM_REFINERY, FUEL_SALES_FUEL_TYPE } from '../models';
import { Page, PageableRequest } from '@sct-shared-lib';

@Injectable({
  providedIn: 'root',
})
export class FuelSalesService {
  private fuelSale$ = new BehaviorSubject<any>({});
  selectedFuelSale$ = this.fuelSale$.asObservable();

  constructor() {}

  getFuelSaleList(pageable: PageableRequest, fuelSalesFilterPayload?: FuelSalesFilter): Observable<Page<FuelSalesResponse>> {
    const { page, size, sort, term } = pageable;
    const { startDate, endDate, fuelType, refinery } = fuelSalesFilterPayload || {};
    const dataRefreshed = [...FUEL_SALES_LIST].map((data) => ({ ...data })).slice((page - 1) * size, (page - 1) * size + size);

    return of({
      content: [...dataRefreshed],
      totalElements: FUEL_SALES_LIST.length,
    } as Page<FuelSalesResponse>);
  }

  getFuelType(): Observable<Page<any>> {
    const fuelTypeData = FUEL_SALES_FUEL_TYPE;
    return of({
      content: [...fuelTypeData],
    } as Page<any>);
  }

  getRefineryState(): Observable<Page<any>> {
    const refinaryStateData = FUEL_SALES_GIM_REFINERY;
    return of({
      content: [...refinaryStateData],
    } as Page<any>);
  }

  setFuelSales(jerrican:any){
    this.fuelSale$.next(jerrican)
  }
}
