import { Component, OnInit } from '@angular/core';
import { Product } from '../../../app/model/product.model';
import { ProductDService } from '../../service/productD.service';
import { ProductCService } from '../../service/productC.service';

@Component({
  selector: 'app-content-projection',
  templateUrl: './content-projection.component.html',
  styleUrls: ['./content-projection.component.css'],
})
export class ContentProjectionComponent implements OnInit {
  numOfProductInfo: string;

  constructor(private productService: ProductDService) {}

  ngOnInit() {
    this.numOfProductInfo = this.productService.numOfProductInfo;
  }
}
