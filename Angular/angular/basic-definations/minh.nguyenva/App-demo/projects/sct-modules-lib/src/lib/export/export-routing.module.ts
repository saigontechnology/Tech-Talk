import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ExportListComponent } from './containers';
import { ExportFormComponent } from './containers/export-form/export-form.component';
import { ExportComponent } from './export.component';
import { ExportAuthGuard } from './export.guard';

import { PATH } from '@sct-shared-lib';

const routes: Routes = [
  {
    path: '',
    data: {
      showHeader: true,
      breadcrumbs: PATH.EXPORT.displayName,
    },
    component: ExportComponent,
    canActivate: [ExportAuthGuard],
    children: [
      {
        path: '',
        component: ExportListComponent,
      },
      {
        path: PATH.EXPORT_UPDATE.route,
        component: ExportFormComponent,
        data: {
          showHeader: true,
          breadcrumbs: PATH.EXPORT_UPDATE.displayName,
        },
      },
      {
        path: PATH.EXPORT_CREATE.route,
        component: ExportFormComponent,
        data: {
          showHeader: true,
          breadcrumbs: PATH.EXPORT_CREATE.displayName,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ExportRoutingModule {}
