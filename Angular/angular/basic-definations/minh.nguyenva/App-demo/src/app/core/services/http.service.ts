import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Observable, map } from 'rxjs';

import { environment } from '@environments/environment';
import { isBoolean, isNil, isNumber } from 'lodash-es';

import { Page, PageableRequest } from '@sct-shared-lib';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  private readonly _apiUrl: string;

  constructor(private _http: HttpClient) {
    const { apiUrl } = environment.appConfig;
    this._apiUrl = apiUrl;
  }
  getResource(apiPath: string) {
    return this._http.get(`${this._apiUrl}/${apiPath}`).pipe(
      map((res: any) => {
        return res;
      })
    );
  }

  postResource(apiPath: string, body: any) {
    return this._http.post(`${this._apiUrl}/${apiPath}`, body).pipe(map((res: any) => {
      return res;
    }));
  }

  putResource(apiPath: string, body: any) {
    return this._http.put(`${this._apiUrl}/${apiPath}`, body).pipe(map((res: any) => {
      return res;
    }));;
  }

  deleteResource(apiPath: string) {
    return this._http.delete(`${this._apiUrl}/${apiPath}`).pipe(map((res: any) => {
      return res;
    }));;
  }

  getResourceByPaging(apiPath: string, pagable?: PageableRequest) {
    let params = {};
    if(pagable){
        params = new HttpParams({
        fromObject: {
          page: pagable.page.toString(),
          size: pagable.size.toString(),
          sort: '',
        },
      });
    }

    return this._http.get(`${this._apiUrl}/${apiPath}`, { params }).pipe(
      map((res: any) => {
        return res;
      })
    );
  }

  getResourceByPayload<T, O>(apiPath: string, payload?: T, headerObj?: any): Observable<any> {
    let headers = new HttpHeaders();
    headers = headers.set(headerObj.id, headerObj.value);

    const fromObject: { [param: string]: string | string[] } = {};
    for (const [key, value] of Object.entries<string | number | string[] | number[]>(payload || {})) {
      if (!isNil(value)) {
        if (isNumber(value) || isBoolean(value)) {
          fromObject[key] = value.toString();
        } else {
          fromObject[key] = value as string;
        }
      }
    }
    return this._http.get<any>(`${this._apiUrl}/${apiPath}`, {
      params: { ...fromObject },
      headers: headers,
    });
  }
}
