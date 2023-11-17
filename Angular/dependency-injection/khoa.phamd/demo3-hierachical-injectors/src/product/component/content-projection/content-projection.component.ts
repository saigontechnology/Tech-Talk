import { Component, OnInit } from '@angular/core';
import { Product } from '../../../app/model/product.model';
import { ProductBService } from '../../service/productB.service';
import { ProductCService } from '../../service/productC.service';
import { ProductAService } from '../../../app/service/productA.service';

@Component({
  selector: 'app-content-projection',
  templateUrl: './content-projection.component.html',
  styleUrls: ['./content-projection.component.css'],
})
export class ContentProjectionComponent implements OnInit {
  // numOfProductInfo: string;
  numOfProductInfo2: string;

  constructor(
    // private productBService: ProductBService,
    private productAService: ProductAService
  ) {}

  ngOnInit() {
    // this.numOfProductInfo = this.productBService.numOfProductInfo;
    this.numOfProductInfo2 = this.productAService.numOfProductInfo;
  }
}
