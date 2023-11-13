import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardContentComponent } from './containers';

import { SharedModule } from '@sct-shared-lib';
import { DasboardAuthGuard } from './dashboard.guard';

const DASHBOARD_COMPONENTS = [DashboardComponent,DashboardContentComponent];

@NgModule({
  declarations: [...DASHBOARD_COMPONENTS],
  imports: [CommonModule, SharedModule, DashboardRoutingModule],
  exports: [...DASHBOARD_COMPONENTS],
  providers: [DasboardAuthGuard],
  
})
export class DashboardModule {}
