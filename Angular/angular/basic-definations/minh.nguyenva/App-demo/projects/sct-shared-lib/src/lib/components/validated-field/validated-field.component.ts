import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'sct-validated-field',
  templateUrl: './validated-field.component.html',
  styleUrls: ['./validated-field.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ValidatedFieldComponent implements OnInit {
  @Input() errors!: any;
  @Input() fieldChange! :boolean | undefined;


  constructor() {}
  ngOnInit(): void {
    }
}
