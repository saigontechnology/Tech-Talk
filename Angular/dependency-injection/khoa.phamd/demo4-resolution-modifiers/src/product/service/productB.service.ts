import { Product } from '../../app/model/product.model';

export class ProductBService {
  private _randomNo = 0;

  constructor() {
    this._randomNo = Math.floor(Math.random() * 1000);
  }

  get numOfProductInfo() {
    return 'Product B: ' + this._randomNo;
  }
}
