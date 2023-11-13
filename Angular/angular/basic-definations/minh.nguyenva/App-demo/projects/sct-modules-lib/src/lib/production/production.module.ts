import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductionListComponent ,ProductionFormComponent} from './containers';
import { ProductionComponent } from './production.component';
import { ProductiontRoutingModule } from './production-routing.module';
import { ProductionAuthGuard } from './production.guard';

import { SharedModule } from '@sct-shared-lib';

const PRODUCTION_COMPONENTS = [ProductionComponent, ProductionListComponent,ProductionFormComponent];

@NgModule({
  declarations: [...PRODUCTION_COMPONENTS],
  imports: [CommonModule, ProductiontRoutingModule, SharedModule],
  exports: [...PRODUCTION_COMPONENTS],
  providers: [ProductionAuthGuard],

})
export class ProductionModule {}
