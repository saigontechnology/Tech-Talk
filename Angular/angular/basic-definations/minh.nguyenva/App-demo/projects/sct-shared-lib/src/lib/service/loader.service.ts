import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  public loader$ = new BehaviorSubject<any>(false);

  constructor() {
  }

  showLoader() {
    this.loader$.next(true);
  }

  hideLoader() {
    this.loader$.next(false);
  }
}
