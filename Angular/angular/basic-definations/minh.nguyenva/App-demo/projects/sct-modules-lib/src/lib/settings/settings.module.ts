import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsRoutingModule } from './settings-routing.module';
import { SettingsComponent } from './settings.component';
import { SettingsFormComponent } from './containers';
import { SettingsAuthGuard } from './settings.guard';

import { SharedModule } from '@sct-shared-lib';

const SETTINGS_COMPONENTS = [SettingsComponent, SettingsFormComponent];

@NgModule({
  declarations: [...SETTINGS_COMPONENTS],
  imports: [CommonModule, SharedModule, SettingsRoutingModule],
  exports: [...SETTINGS_COMPONENTS],
  providers: [SettingsAuthGuard],

})
export class SettingsModule {}
