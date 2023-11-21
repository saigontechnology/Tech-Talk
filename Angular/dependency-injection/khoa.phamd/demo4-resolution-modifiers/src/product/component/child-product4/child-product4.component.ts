import { Component, OnInit } from '@angular/core';
import { ProductEService } from '../../service/productE.service';

@Component({
  selector: 'app-child-product4',
  templateUrl: './child-product4.component.html',
  styleUrls: ['./child-product4.component.scss'],
  // providers: [ProductEService], // @Host Ex2-C2
})
export class ChildProduct4Component implements OnInit {
  constructor() {}

  ngOnInit() {}
}
