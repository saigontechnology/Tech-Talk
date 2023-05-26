import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { CommonService } from '@app/common/common.service';
import { IdentityService } from '@auth/identity/identity.service';

@Injectable({
  providedIn: 'root'
})
export class HttpHelperService {

  constructor(private _router: Router,
    private _identityService: IdentityService,
    private _commonService: CommonService) { }

  handleCommonError(error: any) {
    if (error.status === 401) {
      this._identityService.login(this._router.url);
    } else if (error.status === 403) {
      this._commonService.forbidResult();
    }
  }
}
