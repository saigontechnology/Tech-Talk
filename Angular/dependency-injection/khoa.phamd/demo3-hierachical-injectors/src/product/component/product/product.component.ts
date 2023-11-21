import { Component, OnInit } from '@angular/core';
import { ProductAService } from '../../../../src/app/service/productA.service';
import { Product } from '../../../app/model/product.model';
import { ProductBService } from '../../service/productB.service';
import { ProductCService } from '../../service/productC.service';
import { ProductDService } from '../../service/productD.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  providers: [ProductDService],
  viewProviders: [ProductAService], // Test case Content Projection
})
export class ProductComponent implements OnInit {
  numOfProductInfo: string;
  numOfProductInfo2: string;
  numOfProductInfo3: string;

  constructor(
    private productCService: ProductCService,
    private productDService: ProductDService,
    private productAService: ProductAService
  ) {}

  ngOnInit() {
    this.numOfProductInfo = this.productCService.numOfProductInfo;
    this.numOfProductInfo2 = this.productDService.numOfProductInfo;
    this.numOfProductInfo3 = this.productAService.numOfProductInfo;
  }
}
