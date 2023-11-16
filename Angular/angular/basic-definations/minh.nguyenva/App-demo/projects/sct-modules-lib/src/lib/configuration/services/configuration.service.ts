import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { CONFIGURATION_UNIT } from '../model/configuration.model';

import { Page } from '@sct-shared-lib';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {

constructor() { }
getUnit(): Observable<Page<any>> {
  const unit = CONFIGURATION_UNIT;
  return of({
    content: [...unit],
  } as Page<any>);
}

}
