import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject, takeUntil } from 'rxjs';

import { NzMessageService } from 'ng-zorro-antd/message';
import { NzTableQueryParams } from 'ng-zorro-antd/table';

import { OrderModel } from 'src/app/models/order.model';

import { OrderService } from 'src/app/services/order.service';
import { AppStateService } from 'src/app/services/app-state.service';

@Component({
  selector: 'app-order-table',
  templateUrl: './order-table.component.html',
  styleUrls: ['./order-table.component.scss']
})
export class OrderTableComponent implements OnInit, OnDestroy {

  orderSelections: boolean[];
  selectedOrders: OrderModel[];
  loading: boolean;
  total: number;
  orders?: OrderModel[];

  private _destroy$: Subject<any>;

  constructor(private _orderService: OrderService,
    private _messageService: NzMessageService,
    private _appStateService: AppStateService) {
    this.orderSelections = [];
    this.selectedOrders = [];
    this.loading = false;
    this._destroy$ = new Subject();
    this.total = 0;
  }

  ngOnDestroy(): void {
    this._destroy$.next(true);
  }

  ngOnInit(): void {
    this._handleNewOrder();
    this._handlePromotionApplied();
    this._handleShipApplied();
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    console.log(params);
    const { pageIndex } = params;
    this._fetchOrders(pageIndex - 1);
  }

  private _fetchOrders(page: number) {
    this.loading = true;
    const finishFetching = () => this.loading = false;
    this._orderService.getOrders({
      page,
      pageSize: 10
    }).subscribe({
      next: orderPaging => {
        this.orders = orderPaging.items;
        this.total = orderPaging.total;
      },
      error: () => {
        this._messageService.error('Error fetching orders');
        finishFetching();
      },
      complete: () => finishFetching()
    });
  }

  private _handleNewOrder() {
    this._appStateService.newOrder$
      .pipe(takeUntil(this._destroy$))
      .subscribe(newOrder => {
        if (this.orders && this.orders.findIndex(o => o.id === newOrder.id) < 0) {
          this.orders = [newOrder, ...this.orders];
          this.total += 1;
        }
      });
  }

  private _handlePromotionApplied() {
    this._appStateService.promotionApplied$
      .pipe(takeUntil(this._destroy$))
      .subscribe(event => {
        if (this.orders) {
          const appliedOrder = this.orders.find(o => o.id === event.orderId);
          if (appliedOrder) {
            appliedOrder.discount = event.discount;
          }
        }
      });
  }

  private _handleShipApplied() {
    this._appStateService.shipApplied$
      .pipe(takeUntil(this._destroy$))
      .subscribe(event => {
        if (this.orders) {
          const appliedOrder = this.orders.find(o => o.id === event.orderId);
          if (appliedOrder) {
            appliedOrder.shipAmount = event.shipAmount;
          }
        }
      });
  }
}
