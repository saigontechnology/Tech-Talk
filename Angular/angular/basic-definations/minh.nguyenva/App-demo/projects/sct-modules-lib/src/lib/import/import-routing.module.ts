import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ImportFormComponent, ImportListComponent } from './containers';
import { ImportComponent } from './import.component';

import { PATH } from '@sct-shared-lib';
import { ImportAuthGuard } from './import.guard';

const routes: Routes = [
  {
    path: '',
    data: {
      showHeader: true,
      breadcrumbs: PATH.IMPORT.displayName,
    },
    component: ImportComponent,
    canActivate: [ImportAuthGuard],
    children: [
      {
        path: '',
        component: ImportListComponent,
      },
      {
        path: PATH.IMPORT_UPDATE.route,
        component: ImportFormComponent,
        data: {
          showHeader: true,
          breadcrumbs: PATH.IMPORT_UPDATE.displayName,
        },
      },
      {
        path: PATH.IMPORT_CREATE.route,
        component: ImportFormComponent,
        data: {
          showHeader: true,
          breadcrumbs: PATH.IMPORT_CREATE.displayName,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ImportRoutingModule {}
