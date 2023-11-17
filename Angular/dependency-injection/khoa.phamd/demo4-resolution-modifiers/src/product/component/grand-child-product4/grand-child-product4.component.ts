import { Component, Host, OnInit } from '@angular/core';
import { ProductEService } from '../../service/productE.service';

@Component({
  selector: 'app-grand-child-product4',
  templateUrl: './grand-child-product4.component.html',
  styleUrls: ['./grand-child-product4.component.css'],
  // providers: [ProductEService], // @Host Ex2-C1 Content Projection
})
export class GrandChildProduct4Component implements OnInit {
  numOfProductInfo: string;

  constructor(@Host() private productService: ProductEService) {}

  ngOnInit() {
    this.numOfProductInfo = this.productService.numOfProductInfo;
  }
}
