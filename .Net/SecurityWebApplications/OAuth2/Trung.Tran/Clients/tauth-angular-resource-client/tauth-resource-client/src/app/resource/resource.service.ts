import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { environment } from '@environments/environment';

import { ResourceListItemModel } from './models/resource-list-item.model';
import { ResourceDetailModel } from './models/resource-detail.model';
import { CreateResourceModel } from './models/create-resource-model';

@Injectable({
  providedIn: 'root'
})
export class ResourceService {

  constructor(private _httpClient: HttpClient) { }

  getResourceList(): Observable<ResourceListItemModel[]> {
    return this._httpClient.get<ResourceListItemModel[]>(`${environment.resourceApiUrl}/api/resources`);
  };

  getResourceById(id: number): Observable<ResourceDetailModel> {
    return this._httpClient.get<ResourceDetailModel>(`${environment.resourceApiUrl}/api/resources/${id}`);
  };

  createResource(model: CreateResourceModel): Observable<number> {
    return this._httpClient.post<number>(`${environment.resourceApiUrl}/api/resources`, model);
  };

  deleteResource(id: number): Observable<any> {
    return this._httpClient.delete(`${environment.resourceApiUrl}/api/resources/${id}`);
  };
}
