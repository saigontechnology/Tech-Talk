import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfigurationFormComponent } from './containers';
import { ConfigurationComponent } from './configuration.component';
import { ConfigurationRoutingModule } from './configuration-routing.module';
import { SharedModule } from '@sct-shared-lib';
import { ConfigurationAuthGuard } from './configuration.guard';


@NgModule({
  declarations: [
    ConfigurationComponent,
    ConfigurationFormComponent
  ],
  imports: [
    CommonModule,SharedModule,ConfigurationRoutingModule
  ],
  providers: [ConfigurationAuthGuard],

})
export class ConfigurationModule { }
