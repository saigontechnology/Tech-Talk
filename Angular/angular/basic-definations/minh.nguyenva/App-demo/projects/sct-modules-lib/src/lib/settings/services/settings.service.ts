import { Injectable } from '@angular/core';
import { Observable,of } from 'rxjs';
import { SETTINGS_COUNTRY } from './../models/settings.model';

import { Page, PageableRequest } from '@sct-shared-lib';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

constructor() { }

getCountry(): Observable<Page<any>> {
  const country = SETTINGS_COUNTRY;
  return of({
    content: [...country],
  } as Page<any>);
}

}
