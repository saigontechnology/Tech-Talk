import { Component, OnInit } from '@angular/core';
import { ProductDService } from '../../service/productD.service';
import { ProductCService } from '../../service/productC.service';

@Component({
  selector: 'app-child-product3',
  templateUrl: './child-product3.component.html',
  styleUrls: ['./child-product3.component.scss'],
  // providers: [ProductDService], //  @Host Ex1-C2: use viewProviders in HostComponent, NOT providers
  viewProviders: [ProductDService],
})
export class ChildProduct3Component implements OnInit {
  numOfProductInfo: string;

  constructor(private productService: ProductDService) {}

  ngOnInit() {
    this.numOfProductInfo = this.productService.numOfProductInfo;
  }
}
