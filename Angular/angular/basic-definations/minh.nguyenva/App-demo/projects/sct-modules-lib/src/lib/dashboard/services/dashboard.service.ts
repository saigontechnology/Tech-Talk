import { Injectable } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { IMPORT_DECLARE_DATA_YEARLY, IMPORT_DECLARE_DATA_MONTHLY, GASOLINE_LEVEL, LEVEL_PROGRESS_RESPONSE, INVENTORY_PROGRESS_RESPONSE, GASOLINE_INVENTORY, IMPORT_DECLARE_DATA_QUATERLY } from './../models/dashboard.model';
import { CHART_DATA_RESPONSE, IMPORT_DECLARE_DATA_DAILY } from '../models/dashboard.model';

import { SCHEDULE_TYPES } from '@sct-shared-lib';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  constructor() {}

  getImportDeclaration(scheduleType: string): Observable<CHART_DATA_RESPONSE[]> {
    let data;
    const params: HttpParams = new HttpParams({
      fromObject: {
        scheduleType,
      },
    });

    // temp demo
    switch (scheduleType) {
      case SCHEDULE_TYPES.DAILY.name: {
        data = [...IMPORT_DECLARE_DATA_DAILY];
        break;
      }
      case SCHEDULE_TYPES.MONTHLY.name: {
        data = [...IMPORT_DECLARE_DATA_MONTHLY];
        break;
      }
      case SCHEDULE_TYPES.QUARTERLY.name: {
        data = [...IMPORT_DECLARE_DATA_QUATERLY];
        break;
      }
      case SCHEDULE_TYPES.YEARLY.name: {
        data = [...IMPORT_DECLARE_DATA_YEARLY];
        break;
      }
    }

    return of(data as any);
  }
  getExportDeclaration(scheduleType: string): Observable<CHART_DATA_RESPONSE[]> {
    let data;
    const params: HttpParams = new HttpParams({
      fromObject: {
        scheduleType,
      },
    });

    // temp demo
    switch (scheduleType) {
      case SCHEDULE_TYPES.DAILY.name: {
        data = [...IMPORT_DECLARE_DATA_DAILY];
        break;
      }
      case SCHEDULE_TYPES.MONTHLY.name: {
        data = [...IMPORT_DECLARE_DATA_MONTHLY];
        break;
      }
      case SCHEDULE_TYPES.YEARLY.name: {
        data = [...IMPORT_DECLARE_DATA_YEARLY];
        break;
      }
    }

    return of(data as any);
  }

  getGasolineLevel():Observable<LEVEL_PROGRESS_RESPONSE[]>{
    const data = GASOLINE_LEVEL;
    return  of(data);
  }
  getDieselLevel():Observable<LEVEL_PROGRESS_RESPONSE[]>{
    const data = GASOLINE_LEVEL;
    return  of(data);
  }

  getGasolineInventory():Observable<INVENTORY_PROGRESS_RESPONSE[]>{
    const data = GASOLINE_INVENTORY;
    return  of(data);
  }
  getDieselInventory():Observable<INVENTORY_PROGRESS_RESPONSE[]>{
    const data = GASOLINE_INVENTORY;
    return  of(data);
  }
}