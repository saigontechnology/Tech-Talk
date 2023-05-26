import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthModule } from '@auth/auth.module';

import { Observable } from 'rxjs';

import { environment } from '@environments/environment';

import { UserProfileItemModel } from './models/user-profile-item.model';

@Injectable({
  providedIn: AuthModule
})
export class UserService {

  constructor(private _httpClient: HttpClient) {
  }

  getUserProfile(): Observable<UserProfileItemModel[]> {
    return this._httpClient.get<UserProfileItemModel[]>(`${environment.resourceApiUrl}/api/users/profile`);
  }
}
