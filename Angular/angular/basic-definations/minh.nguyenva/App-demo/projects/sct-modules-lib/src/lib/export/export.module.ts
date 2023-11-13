
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExportRoutingModule } from './export-routing.module';
import { ExportComponent } from './export.component';
import { ExportListComponent } from './containers/export-list/export-list.component';
import { ExportFormComponent } from './containers/export-form/export-form.component';

import { SharedModule } from '@sct-shared-lib';
import { ExportAuthGuard } from './export.guard';

const EXPORT_COMPONENTS = [ExportListComponent, ExportComponent ,ExportFormComponent];

@NgModule({
  declarations: [...EXPORT_COMPONENTS],
  imports: [CommonModule,SharedModule,ExportRoutingModule],
  exports: [...EXPORT_COMPONENTS],
  providers: [ExportAuthGuard],

})
export class ExportModule {}
