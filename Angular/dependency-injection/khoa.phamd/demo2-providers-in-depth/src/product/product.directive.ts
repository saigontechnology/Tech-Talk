import {
  Directive,
  ElementRef,
  HostListener,
  Renderer2,
  Input,
  OnInit,
} from '@angular/core';
import { ProductBService } from './service/productB.service';

@Directive({
  selector: '[appProduct]',
  // providers: [ProductBService],
})
export class ProductDirective {
  span: any;
  text: any;
  parent: any;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2,
    private productBService: ProductBService
  ) {}

  ngOnInit() {
    this.span = this.renderer.createElement('span');
    this.text = this.renderer.createText(
      'Directive of Child Component Content (Same instance with Child Component): ' +
        this.productBService.numOfProductInfo
    );
    this.parent = this.renderer.parentNode(this.el.nativeElement);
  }

  @HostListener('mouseover')
  onMouseOver() {
    this.renderer.appendChild(this.span, this.text);
    this.renderer.appendChild(this.parent, this.span);
  }

  @HostListener('mouseleave')
  onMouseLeave() {
    this.renderer.removeChild(this.parent, this.span);
  }

  @HostListener('click', ['$event']) onClick(event) {
    console.log('Directive onclicked event: ');
    console.log(event);
  }
}
