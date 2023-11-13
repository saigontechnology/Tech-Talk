import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
@Component({
  selector: 'sct-export',
  templateUrl: './export.component.html',
  styleUrls: ['./export.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExportComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
