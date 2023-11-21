import { Component, OnInit } from '@angular/core';
// import { ProductAService } from '../../../../src/app/service/productA.service';
import { Product } from '../../../app/model/product.model';

import { ProductBService } from '../../service/productB.service';
import { ProductCService } from '../../service/productC.service';

@Component({
  selector: 'app-child-product',
  templateUrl: './child-product.component.html',
  styleUrls: ['./child-product.component.scss'],
  providers: [ProductBService],
  // viewProviders: [ProductBService],
})
export class ChildProductComponent implements OnInit {
  numOfProductInfo: string;

  constructor(private productService: ProductBService) {}

  ngOnInit() {
    this.numOfProductInfo = this.productService.numOfProductInfo;
  }
}
