import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FuelSalesListComponent, FuelSalesFormComponent } from './containers';
import { FuelSalesComponent } from './fuel-sales.component';
import { FuelSalesAuthGuard } from './fuel-sales.guard';

import { PATH } from '@sct-shared-lib';

const routes: Routes = [
  {
    path: '',
    data: {
      showHeader: true,
      breadcrumbs: PATH.FUEL_SALES.displayName,
    },
    component: FuelSalesComponent,
    canActivate: [FuelSalesAuthGuard],
    children: [
      {
        path: '',
        component: FuelSalesListComponent,
      },
      {
        path: PATH.FUEL_SALES_UPDATE.route,
        component: FuelSalesFormComponent,
        data: {
          showHeader: true,
          breadcrumbs: PATH.FUEL_SALES_UPDATE.displayName,
        },
      },
      {
        path: PATH.FUEL_SALES_CREATE.route,
        component: FuelSalesFormComponent,
        data: {
          showHeader: true,
          breadcrumbs: PATH.FUEL_SALES_CREATE.displayName,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FuelSalesRoutingModule {}
