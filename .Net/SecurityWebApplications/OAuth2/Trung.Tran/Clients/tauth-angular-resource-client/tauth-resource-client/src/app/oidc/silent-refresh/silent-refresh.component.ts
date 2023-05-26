import { Component, OnInit } from '@angular/core';

import { IdentityService } from '@auth/identity/identity.service';

@Component({
  selector: 'app-silent-refresh',
  templateUrl: './silent-refresh.component.html',
  styleUrls: ['./silent-refresh.component.scss']
})
export class SilentRefreshComponent implements OnInit {

  constructor(private _identityService: IdentityService) { }

  ngOnInit(): void {
    this._identityService.signinSilentCallback();
  }

}
