import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, PRIMARY_OUTLET, Router } from '@angular/router';

import { filter, takeUntil } from 'rxjs/operators';
import { forkJoin, of, Subject } from 'rxjs';

import { CoreService } from '@core/services';

import { BreadcrumbService, Breadcrumb, AuthService, APPLICATION_ROLES } from '@sct-shared-lib';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less'],
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'sct-web-client';

  private readonly _destroy$ = new Subject<void>();
  constructor(
    private _breadcrumbService: BreadcrumbService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    private _coreService: CoreService,
    private _authService: AuthService
  ) {}

  ngOnInit() {
    this._getInitialData();

    const navigationEnd$ = this._router.events.pipe(filter((e) => e instanceof NavigationEnd));

    navigationEnd$.subscribe(() => {
      this._breadcrumbService.setCustomParameters([...this._getBreadcrumbs(this._activatedRoute.children)]);
    });
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.unsubscribe();
  }

  private _getBreadcrumbs(routes: ActivatedRoute[], url: string = '', breadcrumbs: Breadcrumb[] = []): Breadcrumb[] {
    const ROUTE_DATA_BREADCRUMB = 'breadcrumbs';
    const children: ActivatedRoute[] = routes;

    if (children.length === 0) {
      return breadcrumbs;
    }

    for (const child of children) {
      if (child.outlet !== PRIMARY_OUTLET) {
        continue;
      }

      const routeURL: string = child.snapshot.url.map((segment) => segment.path).join('/');
      if (routeURL) {
        url += `/${routeURL}`;
      }

      if (!(child.snapshot?.routeConfig?.data && child?.snapshot?.routeConfig?.data.hasOwnProperty(ROUTE_DATA_BREADCRUMB))) {
        return this._getBreadcrumbs(child.children, url, breadcrumbs);
      }
      // const displayName$ = this._translateService.stream(child.snapshot.routeConfig.data[ROUTE_DATA_BREADCRUMB]);
      const displayName$ = of(child.snapshot.routeConfig.data[ROUTE_DATA_BREADCRUMB]);
      const breadcrumb: Breadcrumb = {
        params: child.snapshot.params,
        path: url,
        displayName$,
      };
      if (breadcrumb.displayName$) {
        breadcrumbs.push(breadcrumb);
      }
      return this._getBreadcrumbs(child.children, url, breadcrumbs);
    }
    return [];
  }

  private _getInitialData() {
    forkJoin([this._coreService.getLocationSite(), this._coreService.getFuelTypes()])
      .pipe(takeUntil(this._destroy$))
      .subscribe(([locationSite, fuelTypes]) => {
        this._coreService.setLocationSite({ loaded: true, content: locationSite });
        this._coreService.setFuelTypes({ loaded: true, content: fuelTypes });
      });

    // if (this._authService.isUserInRole(APPLICATION_ROLES.VIEW_IMPORTS) || this._authService.isUserInRole(APPLICATION_ROLES.VIEW_EXPORTS)) {
    // if (true) {
      // this._coreService
      //   .getCountries()
      //   .pipe(takeUntil(this._destroy$))
      //   .subscribe((countries) => {
      //     this._coreService.setCountries({ loaded: true, content: countries });
      //   });
    // }
  }
}
