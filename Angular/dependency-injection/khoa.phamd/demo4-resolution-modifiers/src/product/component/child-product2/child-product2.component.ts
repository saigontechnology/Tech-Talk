import { Component, OnInit, SkipSelf } from '@angular/core';
import { ProductBService } from '../../service/productB.service';
import { ProductCService } from '../../service/productC.service';

@Component({
  selector: 'app-child-product2',
  templateUrl: './child-product2.component.html',
  styleUrls: ['./child-product2.component.scss'],
  providers: [ProductCService], // Angular igrone this provider (SkipSelf)
})
export class ChildProduct2Component implements OnInit {
  numOfProductInfo: string;

  constructor(@SkipSelf() private productService: ProductCService) {}

  ngOnInit() {
    this.numOfProductInfo = this.productService.numOfProductInfo;
  }
}
