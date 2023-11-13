import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Event, Router, NavigationEnd } from '@angular/router';
import { PATH, UtilitiesService, DATE_FORMAT } from '@sct-shared-lib';
// import { KeycloakService } from 'keycloak-angular';

@Component({
  selector: 'sct-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent implements OnInit {
  buttonText: string = 'This Year';
  isShowWelcome = false;
  time = '';
  date = '';

  userName = '';
  constructor(
    private _router: Router,
    private _utilitiesService: UtilitiesService,
    private _cd: ChangeDetectorRef,
    // private _keyCloakService: KeycloakService
  ) {
    this._watchRoute();
    this._getAccountInfo();
  }
  
  ngOnInit(): void {
    this._getDateTime();
  }

  private _watchRoute() {
    this._router.events.subscribe((event: Event) => {
      if (event instanceof NavigationEnd) {
        if (event.url.includes(PATH.DASHBOARD.route) || event.url == '/') {
          this.isShowWelcome = true;
          return;
        } else {
          this.isShowWelcome = false;
        }
        this._cd.detectChanges();
      }
    });
  }
  private _getDateTime() {
    this.time = this._utilitiesService.formatDateTime(new Date(), DATE_FORMAT.SHORT_TIME);
    this.date = this._utilitiesService.formatDateTime(new Date(), DATE_FORMAT.DATE_TIME_SUFFIX);
  }
  private _getAccountInfo() {
    // this.userName = this._keyCloakService.getUsername();
    return 'Minhnva'
  }
}
