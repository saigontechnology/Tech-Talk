import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable, Subject } from 'rxjs';

import { OrderModel } from '../models/order.model';
import { PromotionAppliedEvent } from '../models/promotion-applied-event.model';
import { ShipAppliedEvent } from '../models/ship-applied-event.model';
import { InteractionReportModel } from '../models/interaction-report.model';
import { UnusualSearchModel } from '../models/unusual-search.model';
import { InteractionModel } from '../models/interaction.model';
import { LogData } from '../models/log-data.model';


@Injectable({
  providedIn: 'root'
})
export class AppStateService {

  private _userName: string;
  private _userName$: BehaviorSubject<string>;
  get userName(): string {
    return this._userName;
  }
  get userName$(): Observable<string> {
    return this._userName$;
  }

  private _lastPage: string;
  private _lastPage$: BehaviorSubject<string>;
  get lastPage(): string {
    return this._lastPage;
  }
  get lastPage$(): Observable<string> {
    return this._lastPage$;
  }

  private _newOrder$: Subject<OrderModel>;
  get newOrder$(): Observable<OrderModel> {
    return this._newOrder$;
  }

  private _promotionApplied$: Subject<PromotionAppliedEvent>;
  get promotionApplied$(): Observable<PromotionAppliedEvent> {
    return this._promotionApplied$;
  }

  private _shipApplied$: Subject<ShipAppliedEvent>;
  get shipApplied$(): Observable<ShipAppliedEvent> {
    return this._shipApplied$;
  }

  private _interactionReportUpdated$: Subject<InteractionReportModel>;
  get interactionReportUpdated$(): Observable<InteractionReportModel> {
    return this._interactionReportUpdated$;
  }

  private _unusualSearchs$: Subject<UnusualSearchModel[]>;
  get unusualSearchs$(): Observable<UnusualSearchModel[]> {
    return this._unusualSearchs$;
  }

  private _newInteractions$: Subject<InteractionModel[]>;
  get newInteractions$(): Observable<InteractionModel[]> {
    return this._newInteractions$;
  }

  private _logData$: Subject<LogData>;
  get logData$(): Observable<LogData> {
    return this._logData$;
  }

  constructor() {
    this._userName = '';
    this._userName$ = new BehaviorSubject<string>(this._userName);
    this._lastPage = '';
    this._lastPage$ = new BehaviorSubject<string>(this._lastPage);
    this._newOrder$ = new Subject<OrderModel>();
    this._promotionApplied$ = new Subject<PromotionAppliedEvent>();
    this._shipApplied$ = new Subject<ShipAppliedEvent>();
    this._interactionReportUpdated$ = new Subject<InteractionReportModel>();
    this._unusualSearchs$ = new Subject<UnusualSearchModel[]>();
    this._newInteractions$ = new Subject<InteractionModel[]>();
    this._logData$ = new Subject<LogData>();
  }

  setUserName(userName: string) {
    this._userName = userName;
    this._userName$.next(userName);
  }

  setLastPage(page: string) {
    this._lastPage = page;
    this._lastPage$.next(page);
  }

  publishNewOrder(order: OrderModel) {
    this._newOrder$.next(order);
  }

  publishPromotionApplied(event: PromotionAppliedEvent) {
    this._promotionApplied$.next(event);
  }

  publishShipApplied(event: ShipAppliedEvent) {
    this._shipApplied$.next(event);
  }

  publishInteractionReportUpdated(report: InteractionReportModel) {
    this._interactionReportUpdated$.next(report);
  }

  publishUnusualSearchs(searchs: UnusualSearchModel[]) {
    this._unusualSearchs$.next(searchs);
  }

  publishNewInteractions(interactions: InteractionModel[]) {
    this._newInteractions$.next(interactions);
  }

  publishNewLogData(logData: LogData) {
    this._logData$.next(logData);
  }
}
