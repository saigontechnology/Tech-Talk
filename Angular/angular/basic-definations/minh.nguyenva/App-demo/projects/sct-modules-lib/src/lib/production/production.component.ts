import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'sct-production',
  templateUrl: './production.component.html',
  styleUrls: ['./production.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductionComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
