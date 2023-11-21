import { Component, OnInit, Optional } from '@angular/core';
import { ProductFService } from '../../service/productF.service';

@Component({
  selector: 'app-grand-child-product5',
  templateUrl: './grand-child-product5.component.html',
  styleUrls: ['./grand-child-product5.component.css'],
  // providers: [ProductFService], // @Host Ex3-C2

})
export class GrandChildProduct5Component implements OnInit {
  numOfProductInfo: string;

  constructor(@Optional() private productService: ProductFService) {}

  ngOnInit() {
    this.numOfProductInfo = this.productService
      ? this.productService.numOfProductInfo
      : 'No Info';
  }
}
