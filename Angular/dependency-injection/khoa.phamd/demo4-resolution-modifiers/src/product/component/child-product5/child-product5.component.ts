import { Component, OnInit } from '@angular/core';
import { ProductFService } from '../../service/productF.service';

@Component({
  selector: 'app-child-product5',
  templateUrl: './child-product5.component.html',
  styleUrls: ['./child-product5.component.scss'],
  // providers: [ProductFService], // @Host Ex3-C3: use viewProviders in HostComponent, NOT providers
  viewProviders: [ProductFService],
})
export class ChildProduct5Component implements OnInit {
  constructor() {}

  ngOnInit() {}
}
