import { Component, OnInit } from '@angular/core';

import { A_ROUTING } from '@app/constants';
import { ROLES } from '@auth/constants';

import { IdentityService } from '@auth/identity/identity.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

  A_ROUTING = A_ROUTING;

  isAuthenticated: boolean;
  isAdmin: boolean;

  constructor(private _identityService: IdentityService) {
    this.isAuthenticated = false;
    this.isAdmin = false;
  }

  ngOnInit(): void {
    this._identityService.getUser().then(user => {
      this.isAuthenticated = !!user;
      this.isAdmin = user?.profile.role === ROLES.Administrator;
    });
  }

  onLogoutClicked() {
    this._identityService.logout();
  }
}
