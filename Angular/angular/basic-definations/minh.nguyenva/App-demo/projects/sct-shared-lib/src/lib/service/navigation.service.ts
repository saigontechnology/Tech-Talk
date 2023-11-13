import { Injectable } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Params, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class NavigationService {
  private history: string[] = [];

  constructor(private _router: Router,    private _activatedRoute: ActivatedRoute,) {
    const navigationEnd$ = this._router.events.pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd));
    navigationEnd$.subscribe((event: NavigationEnd) => {
      const { urlAfterRedirects } = event;
      this.history.push(urlAfterRedirects);
    });
  }

  back(): void {
    this.history.pop();
    if (this.history.length > 0) {
      const url = this.history.pop();
      this._router.navigateByUrl(`${url}`);
    } else {
      this._router.navigateByUrl('/');
    }
  }
  updateRoute(queryParams: Params) {
    this._router.navigate([], {
      queryParams,
      relativeTo: this._activatedRoute,
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }
}
