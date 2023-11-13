import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { APPLICATION_ROLES, ICONS, PATH, SideNavs, AuthService } from '@sct-shared-lib';
@Component({
  selector: 'sct-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideNavComponent implements OnInit {
  @Input() isExpanded: boolean = false;

  @Output() toggleSidebar: EventEmitter<boolean> = new EventEmitter<boolean>();

  sideNavRows: SideNavs[] = [
    { icon: ICONS.DASHBOARD, title: PATH.DASHBOARD.displayName, navLink: PATH.DASHBOARD.route, role: APPLICATION_ROLES.VIEW_DASHBOARD },
    { icon: ICONS.FUEL_SALES, title: PATH.FUEL_SALES.displayName, navLink: PATH.FUEL_SALES.route, role: APPLICATION_ROLES.VIEW_SALES },
    { icon: ICONS.IMPORT, title: PATH.IMPORT.displayName, navLink: PATH.IMPORT.route, role: APPLICATION_ROLES.VIEW_IMPORTS },
    { icon: ICONS.EXPORT, title: PATH.EXPORT.displayName, navLink: PATH.EXPORT.route, role: APPLICATION_ROLES.VIEW_EXPORTS },
    // {
    //   icon: ICONS.PRODUCTION,
    //   title: PATH.PRODUCTION.displayName,
    //   navLink: PATH.PRODUCTION.route,
    //   role: APPLICATION_ROLES.VIEW_PRODUCTIONS,
    // },
    { icon: ICONS.REPORT, title: PATH.REPORTS.displayName, navLink: PATH.REPORTS.route, role: APPLICATION_ROLES.VIEW_REPORTS },
    { icon: ICONS.SETTINGS, title: PATH.SETTINGS.displayName, navLink: PATH.SETTINGS.route, role: APPLICATION_ROLES.VIEW_SETTINGS },
    {
      icon: ICONS.CONFIGURATION,
      title: PATH.CONFIGURATION.displayName,
      navLink: PATH.CONFIGURATION.route,
      role: APPLICATION_ROLES.VIEW_CONFIGURATION,
    },
  ];
  constructor(private _authService: AuthService) {}

  ngOnInit(): void {
    this._isAccountInRole();
  }

  handleSidebarToggle() {
    this.toggleSidebar.emit(!this.isExpanded);
  }

  onLogout() {
    this._authService.logOutSession();
  }

  private _isAccountInRole() {
    this.sideNavRows = this.sideNavRows.filter((item) => this._authService.isUserInRole(item.role));
  }
}
