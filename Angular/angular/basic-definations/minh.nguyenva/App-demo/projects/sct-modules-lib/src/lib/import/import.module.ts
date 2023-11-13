import { ImportAuthGuard } from './import.guard';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImportComponent } from './import.component';
import { ImportRoutingModule } from './import-routing.module';
import { ImportListComponent } from './containers';
import { ImportFormComponent } from './containers/import-form/import-form.component';

import { SharedModule } from '@sct-shared-lib';

const IMPORT_COMPONENTS = [ImportComponent, ImportListComponent, ImportFormComponent];
@NgModule({
  declarations: [...IMPORT_COMPONENTS],
  imports: [CommonModule, ImportRoutingModule, SharedModule],
  exports: [...IMPORT_COMPONENTS],
  providers: [ImportAuthGuard],
})
export class ImportModule {}
