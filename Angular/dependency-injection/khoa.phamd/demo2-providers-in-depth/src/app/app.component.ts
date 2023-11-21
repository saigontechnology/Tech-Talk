import { Component, inject } from '@angular/core';
import { Product } from './model/product.model';
import { ProductAService } from './service/productA.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  numOfProductInfo: string;
  private productService = inject(ProductAService); // C2: Use from Angular v14

  constructor() {}
  // constructor(private productService: ProductAService) {} // C1: Regular way

  ngOnInit() {
    this.numOfProductInfo = this.productService.numOfProductInfo;
  }
}
