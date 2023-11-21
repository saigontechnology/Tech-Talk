import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductRoutingModule } from './product-routing.module';

import { ProductComponent } from './component/product/product.component';
import { ChildProductComponent } from './component/child-product/child-product.component';
import { ChildProduct2Component } from './component/child-product2/child-product2.component';
import { ChildProduct3Component } from './component/child-product3/child-product3.component';
import { ChildProduct4Component } from './component/child-product4/child-product4.component';
import { ChildProduct5Component } from './component/child-product5/child-product5.component';
import { GrandChildProduct3Component } from './component/grand-child-product3/grand-child-product3.component';
import { GrandChildProduct4Component } from './component/grand-child-product4/grand-child-product4.component';
import { GrandChildProduct5Component } from './component/grand-child-product5/grand-child-product5.component';
import { ContentProjectionComponent } from './component/content-projection/content-projection.component';
import { ProductDirective } from './product.directive';

import { ProductCService } from './service/productC.service';
import { ProductDService } from './service/productD.service';

@NgModule({
  imports: [CommonModule, ProductRoutingModule],
  declarations: [
    ProductComponent,
    ChildProductComponent,
    ChildProduct2Component,
    ChildProduct3Component,
    ChildProduct4Component,
    ChildProduct5Component,
    GrandChildProduct3Component,
    GrandChildProduct4Component,
    GrandChildProduct5Component,
    ContentProjectionComponent,
    ProductDirective,
  ],
  providers: [ProductCService, ProductDService],
})
export class ProductModule {}
