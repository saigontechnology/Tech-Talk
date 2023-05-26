import { Injectable } from '@angular/core';
import { AuthModule } from '@auth/auth.module';

import { User, UserManager } from 'oidc-client';

import { ODIC_CONFIG } from '@auth/constants';

@Injectable({
  providedIn: AuthModule
})
export class IdentityService {

  private _userManager: UserManager;
  constructor() {
    this._userManager = new UserManager(ODIC_CONFIG);
    this._userManager.events.addSilentRenewError((error) => {
      console.log(error);
    });
  }

  signinSilentCallback(): Promise<User | undefined> {
    return this._userManager.signinSilentCallback();
  }

  getUser(): Promise<User | null> {
    return this._userManager.getUser();
  }

  storeUser(user: User): Promise<void> {
    return this._userManager.storeUser(user);
  }

  login(returnUrl?: string): Promise<void> {
    return this._userManager.signinRedirect({
      state: {
        returnUrl
      }
    });
  }

  logout(): Promise<void> {
    return this._userManager.signoutRedirect();
  }

  signinRedirectCallback(): Promise<User> {
    return this._userManager.signinRedirectCallback();
  }
}
