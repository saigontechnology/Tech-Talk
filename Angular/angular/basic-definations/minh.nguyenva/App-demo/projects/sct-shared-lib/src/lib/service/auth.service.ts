import { Injectable } from '@angular/core';
import { KeycloakService } from 'keycloak-angular';
import { KeycloakProfile } from 'keycloak-js';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private _keyCloakService: KeycloakService) {}

  // returns a promise
  
  getToken() {
    // return this._keyCloakService.getToken();
  }
  isLoggedIn() {
    // return this._keyCloakService.isLoggedIn();
  }


  loadUserProfile() {
    // return this._keyCloakService.loadUserProfile();
  }
  // returns other type
  logOutSession() {
    // this._keyCloakService.logout();
  }

  getUserName() {
    // return this._keyCloakService.getUsername();
  }

  isUserInRole(role: string) {
    // return this._keyCloakService.isUserInRole(role);
    return true;
  }

  areRolesPresent(...roles: string[]): boolean {
    // return roles.map(role => this._keyCloakService.isUserInRole(role))
    //     .reduce((previousValue, currentValue) => previousValue && currentValue, true);
    return true;
}

}
