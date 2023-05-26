import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { dynamicUrls } from '../constants/urls.const';

import { parseUrl } from '../helpers/url.helper';

import { SubmitOrderModel } from '../models/submit-order.model';
import { SimpleFilterModel } from '../models/simple-filter.model';

import { AppStateService } from './app-state.service';
import { OrderModel } from '../models/order.model';
import { PagingListResponse } from '../models/paging-list-response.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private _httpClient: HttpClient,
    private _appStateService: AppStateService) { }

  submitOrder(orderModel: Partial<SubmitOrderModel>) {
    const url = parseUrl('/api/orders', dynamicUrls.saleApiUrl);
    orderModel.userName = this._appStateService.userName;
    return this._httpClient.post(url.toString(), orderModel);
  }

  getOrders(filter: SimpleFilterModel) {
    const url = parseUrl('/api/orders/filter', dynamicUrls.saleApiUrl);
    return this._httpClient.post<PagingListResponse<OrderModel>>(url.toString(), filter);
  }
}
