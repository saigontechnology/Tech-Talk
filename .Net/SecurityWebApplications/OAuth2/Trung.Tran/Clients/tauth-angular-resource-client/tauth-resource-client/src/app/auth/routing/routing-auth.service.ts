import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from '@angular/router';

import { AuthModule } from '../auth.module';

import { PolicyInjector } from '../policies/policy-injector';

import { A_ROUTING } from '@app/constants';

import { AuthContext } from '../models/auth-context.model';
import { RoutingData } from '@cross/routing/models/routing-data.model';

import { IdentityService } from '@auth/identity/identity.service';
import { CommonService } from '@app/common/common.service';

@Injectable({
  providedIn: AuthModule
})
export class RoutingAuthService implements CanActivate, CanActivateChild {

  constructor(private _router: Router,
    private _identityService: IdentityService,
    private _commonService: CommonService) { }

  async canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean | UrlTree> {
    if (childRoute.routeConfig?.canActivate?.length) return true;
    if (childRoute.parent) return await this.canActivate(childRoute.parent, state);
    return false;
  }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean | UrlTree> {
    const user = await this._identityService.getUser();
    const authContext = new AuthContext(!!user, user);
    const routeData = route.data as RoutingData | undefined;
    const policies = routeData?.policies;

    if (policies) {
      for (let policyType of policies) {
        const policy = PolicyInjector.get(policyType);
        if (!policy) throw new Error(`Policy '${policies}' not found`);
        await policy.authorizeAsync(authContext);
      }

      const authResult = authContext.authResult;

      if (authResult.isSuccess) return true;
      if (authResult.unauthorized) {
        if (routeData?.loginPath) return this._router.parseUrl(routeData.loginPath);
        else {
          await this._identityService.login(state.url);
          return false;
        }
      }

      if (authResult.accessDenied) {
        if (routeData?.accessDeniedPath) {
          this._commonService.accessDenied = true;
          return this._router.parseUrl(routeData.accessDeniedPath);
        } else {
          return this._router.parseUrl(A_ROUTING.notFound);
        }
      }
    }

    return true;
  }
}
