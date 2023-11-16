import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-ecom',
  templateUrl: './ecom.component.html',
  styleUrls: ['./ecom.component.css'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.Default,
})
export class EcomComponent implements OnInit {
  constructor() {}

  ngOnInit() {}
}
