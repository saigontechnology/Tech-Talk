import { Component, OnInit } from '@angular/core';
import { Product } from '../../../app/model/product.model';
import { ProductBService } from '../../service/productB.service';
import { ProductCService } from '../../service/productC.service';
import { ProductEService } from '../../service/productE.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  // providers: [ProductEService], // @Host Ex2-C3: use viewProviders in HostComponent, NOT providers
  viewProviders: [ProductEService],
})
export class ProductComponent implements OnInit {
  numOfProductInfo: string;

  constructor(
    private productCService: ProductCService,
    private productEService: ProductEService
  ) {}

  ngOnInit() {
    this.numOfProductInfo =
      this.productCService.numOfProductInfo +
      ' - ' +
      this.productEService.numOfProductInfo;
  }
}
