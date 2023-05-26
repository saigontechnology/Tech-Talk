import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { NzMessageService } from 'ng-zorro-antd/message';

import { ProductModel } from 'src/app/models/product.model';

import { ProductService } from 'src/app/services/product.service';
import { InteractionService } from 'src/app/services/interaction.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-product-table',
  templateUrl: './product-table.component.html',
  styleUrls: ['./product-table.component.scss']
})
export class ProductTableComponent implements OnInit {

  searchValue: string;
  productSelections: boolean[];
  selectedProducts: ProductModel[];
  loading: boolean;
  products?: ProductModel[];

  private _originalProducts?: ProductModel[];

  constructor(private _productService: ProductService,
    private _interactionService: InteractionService,
    private _orderService: OrderService,
    private _messageService: NzMessageService,
    private _router: Router) {
    this.searchValue = '';
    this.productSelections = [];
    this.selectedProducts = [];
    this.loading = false;
  }

  ngOnInit(): void {
    this.loading = true;
    this._fetchProducts();
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    const filteredProducts = (this._originalProducts || [])
      .filter((item: ProductModel) => item.name.indexOf(this.searchValue) !== -1);
    this.productSelections = filteredProducts.map(_ => false);
    this.products = filteredProducts;
    this._saveSearchInteraction()
  }

  onSelectProduct(checked: boolean, index: number) {
    this._checkSelectedProducts();
  }

  clearSelection() {
    this.productSelections = this.productSelections.map(_ => false);
    this._checkSelectedProducts();
  }

  submitOrder() {
    this._submitCreateOrder(this.selectedProducts);
  }

  createProduct() {
    const name = prompt(`Enter a product name: `)?.trim();
    const price = Number.parseFloat(prompt(`Enter price: `) || '');
    if (!name || isNaN(price)) {
      this._messageService.error('Invalid product data');
    } else {
      const newProduct: Partial<ProductModel> = { name, price };
      this._submitCreateProduct(newProduct);
    }
  }

  openProductUpdate(product: ProductModel) {
    const newName = prompt(`Enter a new name (${product.name}): `)?.trim();
    const newPrice = Number.parseFloat(prompt(`Enter a new price (${product.price}): `) || '');
    if (!newName || isNaN(newPrice)) {
      this._messageService.error('Invalid product data');
    } else {
      const updatedProduct = { ...product, name: newName, price: newPrice };
      this._submitUpdateProduct(updatedProduct);
    }
  }

  private _checkSelectedProducts() {
    this.selectedProducts = this.products?.filter((item, idx) => this.productSelections[idx]) || [];
  }

  private _fetchProducts() {
    const finishFetching = () => this.loading = false;
    this._productService.getProducts({}).subscribe({
      next: products => {
        this._originalProducts = products;
        this.products = products;
        this.productSelections = products.map(_ => false);
      },
      error: () => {
        this._messageService.error('Error fetching products');
        finishFetching();
      },
      complete: () => finishFetching()
    });
  }

  private _submitUpdateProduct(updatedProduct: ProductModel) {
    this.loading = true;

    const submitFinish = () => this.loading = false;

    this._productService.updateProduct(updatedProduct).subscribe({
      next: () => {
        const product = this._originalProducts?.find(p => p.id === updatedProduct.id);
        if (product) {
          Object.assign(product, updatedProduct);
        }

        this._messageService.success('Updated product successfully');
      },
      error: (err) => {
        this._messageService.error('Failed to update product');
        submitFinish();
      },
      complete: () => submitFinish()
    });
  }

  private _submitCreateProduct(newProduct: Partial<ProductModel>) {
    this.loading = true;

    const submitFinish = () => this.loading = false;

    this._productService.createProduct(newProduct).subscribe({
      next: (createdProduct) => {
        this._originalProducts?.push(createdProduct);
        this.reset();
        this._fetchProducts();
        this._messageService.success('Created product successfully');
      },
      error: (err) => {
        this._messageService.error('Failed to create product');
        submitFinish();
      },
      complete: () => submitFinish()
    });
  }

  private _submitCreateOrder(selectedProducts: ProductModel[]) {
    if (!selectedProducts.length) return;

    this.loading = true;

    const submitFinish = () => this.loading = false;

    this._orderService.submitOrder({
      productIds: selectedProducts.map(p => p.id)
    }).subscribe({
      next: () => {
        this._router.navigate(['/', 'orders']);
        this._messageService.success('Submitted order successfully');
      },
      error: (err) => {
        this._messageService.error('Failed to submit order');
        submitFinish();
      },
      complete: () => submitFinish()
    });
  }

  private _saveSearchInteraction() {
    this._interactionService.saveSearch(this.searchValue).subscribe({
      next: () => {
        console.log('Save search ', this.searchValue);
      },
      error: (err) => {
        console.log('Error: ', err);
      }
    });
  }
}
