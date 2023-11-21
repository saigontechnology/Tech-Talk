import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductComponent } from './component/product/product.component';
import { ProductRoutingModule } from './product-routing.module';
import { ProductCService } from './service/productC.service';
import { ChildProductComponent } from './component/child-product/child-product.component';
import { ContentProjectionComponent } from './component/content-projection/content-projection.component';
import { ProductDirective } from './product.directive';

@NgModule({
  imports: [CommonModule, ProductRoutingModule],
  declarations: [
    ProductComponent,
    ChildProductComponent,
    ContentProjectionComponent,
    ProductDirective,
  ],
  providers: [ProductCService],
})
export class ProductModule {}
