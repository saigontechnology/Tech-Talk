import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ControlType ,formControlBase} from './../../models/form-control-base';
import { FormControlService } from './../../service/form-control.service';

@Component({
  selector: 'sct-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FilterComponent implements OnInit {
  @Input() controls!: formControlBase<string>[];

  @Output() changed = new EventEmitter<any>();

  form!: FormGroup;
  readonly controlType = ControlType;

  constructor(private _formService: FormControlService) {}

  ngOnInit(): void {
    this._initForm();
  }

  private _initForm(data?: any) {
    if (!this.form) {
      this.form = this._formService.toFormGroup(this.controls as formControlBase<string>[]);
      this.form.valueChanges.subscribe((filterValue) => {
        if(this.form.valid){
          this.changed.emit(filterValue);
        }
      });

      if (data) {
        this.form.patchValue(data);
      }
    }
  }
}
