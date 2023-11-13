import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ConfigurationComponent } from './configuration.component';
import { ConfigurationFormComponent } from './containers';
import { ConfigurationAuthGuard } from './configuration.guard';

import { PATH } from '@sct-shared-lib';

const routes: Routes = [
  {
    path: '',
    data: {
      showHeader: true,
      breadcrumbs: PATH.CONFIGURATION.displayName,
    },
    component: ConfigurationComponent,
    canActivate: [ConfigurationAuthGuard],
    children: [
      {
        path: '',
        component: ConfigurationFormComponent,
      }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ConfigurationRoutingModule {}
