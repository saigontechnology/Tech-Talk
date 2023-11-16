import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
@Component({
  selector: 'sct-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ImportComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
}
