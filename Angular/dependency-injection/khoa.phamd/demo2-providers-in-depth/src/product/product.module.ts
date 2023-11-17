import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductComponent } from './component/product/product.component';
import { ProductRoutingModule } from './product-routing.module';
import { ProductCService } from './service/productC.service';
import { ChildProductComponent } from './component/child-product/child-product.component';
import { ContentProjectionComponent } from './component/content-projection/content-projection.component';
import { ProductDirective } from './product.directive';
import { ProductBService } from './service/productB.service';
import { ProductDService } from './service/productD.service';
import { ProductEService } from './service/productE.service';
import { ProductFService } from './service/productF.service';
import { ProductGService } from './service/productG.service';


@NgModule({
  imports: [CommonModule, ProductRoutingModule],
  declarations: [
    ProductComponent,
    ChildProductComponent,
    ContentProjectionComponent,
    ProductDirective,
  ],
  providers: [
    ProductBService,
    { provide: ProductCService, useClass: ProductBService },
    { provide: ProductDService, useExisting: ProductBService },
    { provide: ProductEService, useValue: { numOfProductInfo: 'Fake Value: ---' } },
    
    {
      provide: ProductFService,
      useValue: { numOfProductInfo: 'Fake Value: 000' },
      multi: true,
    },
    {
      provide: ProductFService,
      useValue: { numOfProductInfo: 'Fake Value: 999' },
      multi: true,
    },
    {
      provide: ProductGService,
      useFactory: () => {
        console.log('use Factory')
        return { numOfProductInfo: 'Factory New Value: 888' };
      }
    },
  ],
})
export class ProductModule {}
