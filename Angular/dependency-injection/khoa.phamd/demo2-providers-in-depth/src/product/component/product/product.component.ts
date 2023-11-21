import { Component, Inject, OnInit } from '@angular/core';
import { Product } from '../../../app/model/product.model';
import { ProductBService } from '../../service/productB.service';
import { ProductCService } from '../../service/productC.service';
import { ProductDService } from '../../service/productD.service';
import { ProductEService } from '../../service/productE.service';
import { ProductFService } from '../../service/productF.service';
import { ProductGService } from '../../service/productG.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
})
export class ProductComponent implements OnInit {
  numOfProductBInfo: string;
  numOfProductCInfo: string;
  numOfProductDInfo: string;
  numOfProductEInfo: string;
  numOfProductGInfo: string;
  numOfProductFInfo: any[];

  constructor(
    private productBService: ProductBService,
    private productCService: ProductCService,
    private productDService: ProductDService,
    private productEService: ProductEService,
    private productGService: ProductGService,

    @Inject(ProductFService) productFs: any[]
  ) {
    this.numOfProductFInfo = productFs;
  }

  ngOnInit() {
    this.numOfProductBInfo = this.productBService.numOfProductInfo;
    this.numOfProductCInfo = this.productCService.numOfProductInfo;
    this.numOfProductDInfo = this.productDService.numOfProductInfo;
    this.numOfProductEInfo = this.productEService.numOfProductInfo;
    this.numOfProductGInfo = this.productGService.numOfProductInfo;
  }
}
