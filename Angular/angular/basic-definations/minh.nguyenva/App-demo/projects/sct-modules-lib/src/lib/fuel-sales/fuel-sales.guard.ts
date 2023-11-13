import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuard } from '@core/keycloak/app.guard';
import { AuthService, APPLICATION_ROLES } from '@sct-shared-lib';

@Injectable({
  providedIn: 'root',
})
export class FuelSalesAuthGuard extends AuthGuard {
  constructor(authService: AuthService, router: Router) {
    super(router, authService, APPLICATION_ROLES.VIEW_SALES);
  }
}
