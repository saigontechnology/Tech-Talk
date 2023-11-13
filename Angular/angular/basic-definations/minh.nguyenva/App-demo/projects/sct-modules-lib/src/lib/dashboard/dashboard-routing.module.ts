import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardContentComponent } from './containers/dashboard-content/dashboard-content.component';
import { DashboardComponent } from './dashboard.component';

import { PATH } from '@sct-shared-lib';
import { DasboardAuthGuard } from './dashboard.guard';


const routes: Routes = [
  {
    path: '',
    data: {
      showHeader: true,
      breadcrumbs: PATH.DASHBOARD.displayName,
      canActivate: [DasboardAuthGuard],
    },
    component: DashboardComponent,
    children: [
      {
        path: '',
        component: DashboardContentComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DashboardRoutingModule {}
