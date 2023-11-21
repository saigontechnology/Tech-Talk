import { Component, Host, OnInit } from '@angular/core';
import { ProductBService } from '../../service/productB.service';
import { ProductCService } from '../../service/productC.service';
import { ProductDService } from '../../service/productD.service';

@Component({
  selector: 'app-grand-child-product3',
  templateUrl: './grand-child-product3.component.html',
  styleUrls: ['./grand-child-product3.component.css'],
  // providers: [ProductDService], // @Host Ex1 - C1
})
export class GrandChildProduct3Component implements OnInit {
  numOfProductInfo: string;

  constructor(@Host() private productService: ProductDService) {}

  ngOnInit() {
    this.numOfProductInfo = this.productService.numOfProductInfo;
  }
}
