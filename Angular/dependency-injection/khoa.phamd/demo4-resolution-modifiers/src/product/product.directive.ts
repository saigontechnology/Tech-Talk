import {
  Directive,
  ElementRef,
  HostListener,
  Renderer2,
  Input,
  OnInit,
  Host,
} from '@angular/core';
import { ProductFService } from './service/productF.service';

@Directive({
  selector: '[appProduct]',
  // providers: [ProductFService], // @Host Ex3-C1
})
export class ProductDirective {
  span: any;
  text: any;
  parent: any;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2,
    @Host() private productFService: ProductFService
  ) {}

  ngOnInit() {
    this.span = this.renderer.createElement('span');
    this.text = this.renderer.createText(
      'Directive of GrandChild Component Content: ' +
        this.productFService.numOfProductInfo
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
