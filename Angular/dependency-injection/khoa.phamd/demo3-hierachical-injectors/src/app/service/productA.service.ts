import { Injectable } from '@angular/core';
import { Product } from '../model/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductAService {
  private _randomNo = 0;

  constructor() {
    this._randomNo = Math.floor(Math.random() * 1000);
  }

  get numOfProductInfo() {
    return 'Product A: ' + this._randomNo;
  }
}
