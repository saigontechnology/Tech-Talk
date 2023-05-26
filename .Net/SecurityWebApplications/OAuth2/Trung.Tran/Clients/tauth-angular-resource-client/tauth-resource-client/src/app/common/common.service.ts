import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { A_ROUTING } from '@app/constants';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  accessDenied: boolean;

  constructor(private _router: Router) {
    this.accessDenied = false;
  }

  forbidResult() {
    this.accessDenied = true;
    this._router.navigateByUrl(A_ROUTING.accessDenied);
  }
}
