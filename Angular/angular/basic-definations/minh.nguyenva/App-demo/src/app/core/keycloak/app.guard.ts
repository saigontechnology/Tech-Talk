import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, CanActivate, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '@sct-shared-lib';
import { KeycloakAuthGuard, KeycloakService } from 'keycloak-angular';

@Injectable({
  providedIn: 'root',
})
export abstract class AuthGuard implements CanActivate {
  protected roles: string[];

  constructor(protected router: Router, protected _authService: AuthService, @Inject(String) ...roles: string[]) {
    this.roles = roles;
  }

  canActivate(): Promise<boolean> {
    return new Promise((resolve) => {
      if (!this._authService.areRolesPresent(...this.roles)) {
        this.handleViolation(this.roles);
        resolve(false);
        return;
      }

      resolve(true);
    });
  }
  protected handleViolation(requiredRoles: string[]) {
    console.error('Forbidden! Required role(s) not found :', requiredRoles);
    this.router.navigateByUrl('/');
  }
}
