import { Injectable } from '@angular/core';

import { Observable, ReplaySubject } from 'rxjs';
import { Breadcrumb } from './breadcrumb.model';

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {
  private customParams$: ReplaySubject<Breadcrumb[]> = new ReplaySubject<Breadcrumb[]>(1);

  public activeBreadcrumbs$: Observable<Breadcrumb[]> = this.customParams$.asObservable();

  setCustomParameters(customParams: Breadcrumb[]): void {
    this.customParams$.next(customParams);
  }

}
