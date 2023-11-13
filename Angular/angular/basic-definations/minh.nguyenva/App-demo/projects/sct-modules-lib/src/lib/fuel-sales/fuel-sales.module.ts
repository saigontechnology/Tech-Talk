import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FuelSalesComponent } from './fuel-sales.component';
import { FuelSalesListComponent,FuelSalesFormComponent } from './containers';
import { FuelSalesRoutingModule } from './fuel-sales-routing.module';
import { FuelSalesAuthGuard } from './fuel-sales.guard';

import { SharedModule } from '@sct-shared-lib';

@NgModule({
  declarations: [FuelSalesComponent, FuelSalesListComponent, FuelSalesFormComponent],
  imports: [CommonModule, SharedModule,FuelSalesRoutingModule],
  exports: [],
  providers: [FuelSalesAuthGuard],

})
export class FuelSalesModule {}
