export class ProductGService {
  private _randomNo = 0;

  constructor() {
    this._randomNo = Math.floor(Math.random() * 1000);
  }

  get numOfProductInfo() {
    return 'Product G: ' + this._randomNo;
  }
}
