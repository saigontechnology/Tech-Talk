import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SettingsComponent } from './settings.component';
import { SettingsFormComponent} from './containers';
import { SettingsAuthGuard } from './settings.guard';

import { PATH } from '@sct-shared-lib';

const routes: Routes = [
  {
    path: '',
    data: {},
    component: SettingsComponent,
    canActivate: [SettingsAuthGuard],
    children: [
      {
        path: '',
        component: SettingsFormComponent,
        data: {
          showHeader: true,
          breadcrumbs: PATH.SETTINGS.displayName,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SettingsRoutingModule {}
