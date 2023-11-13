import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProductionComponent } from './production.component';
import { ProductionFormComponent,ProductionListComponent } from './containers';
import { ProductionAuthGuard } from './production.guard';

import { PATH } from '@sct-shared-lib';



const routes: Routes = [
  {
    path: '',
    data: {
      showHeader: true,
      breadcrumbs: PATH.PRODUCTION.displayName,
    },
    component: ProductionComponent,
    canActivate: [ProductionAuthGuard],
    children: [
      {
        path: '',
        component: ProductionListComponent,
      },
      {
        path: PATH.PRODUCTION_UPDATE.route,
        component: ProductionFormComponent,
        data: {
          showHeader: true,
          breadcrumbs: PATH.PRODUCTION_UPDATE.displayName,
        },
      },
      {
        path: PATH.PRODUCTION_CREATE.route,
        component: ProductionFormComponent,
        data: {
          showHeader: true,
          breadcrumbs: PATH.PRODUCTION_CREATE.displayName,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProductiontRoutingModule {}
