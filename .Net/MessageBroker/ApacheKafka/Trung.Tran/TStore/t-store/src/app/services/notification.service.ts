import { Injectable } from '@angular/core';

import * as signalR from '@microsoft/signalr';
import { NzMessageService } from 'ng-zorro-antd/message';

import { NotificationType } from '../constants/notification.const';
import { dynamicUrls } from '../constants/urls.const';

import { parseUrl } from '../helpers/url.helper';

import { InteractionReportModel } from '../models/interaction-report.model';
import { InteractionModel } from '../models/interaction.model';
import { LogData } from '../models/log-data.model';
import { NotificationModel } from '../models/notification.model';
import { OrderModel } from '../models/order.model';
import { PromotionAppliedEvent } from '../models/promotion-applied-event.model';
import { ShipAppliedEvent } from '../models/ship-applied-event.model';
import { UnusualSearchModel } from '../models/unusual-search.model';

import { AppStateService } from './app-state.service';


@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private _connection: signalR.HubConnection;

  constructor(private _appStateService: AppStateService,
    private _messageService: NzMessageService) {
    const notiHubUrl = parseUrl('/noti', dynamicUrls.realtimeApiUrl)
    this._connection = new signalR.HubConnectionBuilder()
      .withUrl(notiHubUrl.toString())
      .build();
  }


  listenNotifications() {
    this._connection.on("HandleNotification",
      (notification: NotificationModel) => this._handleNotification(notification));

    this._connection.start().then(() => {
      this._messageService.success('Connected to Realtime API');
    }).catch((err) => {
      this._messageService.error('Cannot connect Realtime API');
    });
  }

  _handleNotification(notification: NotificationModel) {
    switch (notification.type) {
      case NotificationType.NewOrder: {
        const order: OrderModel = notification.data;
        this._appStateService.publishNewOrder(order);
        break;
      }
      case NotificationType.PromotionApplied: {
        const event: PromotionAppliedEvent = notification.data;
        this._appStateService.publishPromotionApplied(event);
        break;
      }
      case NotificationType.ShipApplied: {
        const event: ShipAppliedEvent = notification.data;
        this._appStateService.publishShipApplied(event);
        break;
      }
      case NotificationType.InteractionReportUpdated: {
        const report: InteractionReportModel = notification.data;
        this._appStateService.publishInteractionReportUpdated(report);
        break;
      }
      case NotificationType.UnusualSearchs: {
        const searchs: UnusualSearchModel[] = notification.data;
        this._appStateService.publishUnusualSearchs(searchs);
        break;
      }
      case NotificationType.NewInteractions: {
        const interactions: InteractionModel[] = notification.data;
        this._appStateService.publishNewInteractions(interactions);
        break;
      }
      case NotificationType.Log: {
        const logData: LogData = notification.data;
        this._appStateService.publishNewLogData(logData);
        break;
      }
      default:
        this._messageService.error('Unknown notification type');
        break;
    }
  }
}
