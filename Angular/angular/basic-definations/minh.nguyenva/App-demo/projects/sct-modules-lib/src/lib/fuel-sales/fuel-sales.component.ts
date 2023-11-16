import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'sct-fuel-sales',
  templateUrl: './fuel-sales.component.html',
  styleUrls: ['./fuel-sales.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FuelSalesComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
