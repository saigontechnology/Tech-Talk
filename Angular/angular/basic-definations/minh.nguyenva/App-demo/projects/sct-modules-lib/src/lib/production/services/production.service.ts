import { Injectable } from '@angular/core';
import { of, Observable, BehaviorSubject } from 'rxjs';
import { ProductionPayLoad, ProductionListResponse, ProductionListRequest } from '../model';

import { Page, PageableRequest } from '@sct-shared-lib';
import { HttpService } from '@core/services';

@Injectable({
  providedIn: 'root',
})
export class ProductionService {
  private selectedProduction = new BehaviorSubject<any>({});
  selectedProduction$ = this.selectedProduction.asObservable();

  constructor(private _httpService: HttpService) {
    const productionStored = JSON.parse(localStorage.getItem('PRODUCTION') || '{}');
    if (productionStored) {
      this.selectedProduction.next(productionStored);
    }
  }

  getProductionList(pageable: PageableRequest, payload?: ProductionPayLoad, siteId?: string): Observable<Page<ProductionListResponse>> {
    const headerObj = { id: 'siteId', value: siteId };
    return this._httpService.getResourceByPayload<PageableRequest & ProductionPayLoad, Page<ProductionListResponse>>(
      'productions',
      {
        ...pageable,
        ...payload,
      },
      headerObj
    );
  }

  setProduction(selectedProduction: any) {
    this.selectedProduction.next(selectedProduction);
    localStorage.setItem('PRODUCTION', JSON.stringify(selectedProduction));
  }

  createProduction(body: ProductionListRequest): Observable<any> {
    return this._httpService.postResource('productions', { ...body });
  }

  updateProduction(importId: string, body: ProductionListRequest): Observable<any> {
    return this._httpService.putResource(`productions/${importId}`, { ...body });
  }

  deleteProduction(importId: string): Observable<any> {
    return this._httpService.deleteResource(`productions/${importId}`);
  }
}
