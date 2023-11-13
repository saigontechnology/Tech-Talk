import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ChangeDetectionStrategy, Component, OnInit, forwardRef, ChangeDetectorRef} from '@angular/core';
import { NgbDateStruct, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from './filter.datepicker';

@Component({
  selector: 'sct-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DatePickerComponent),
      multi: true,
    },
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter },
  ],
})
export class DatePickerComponent implements ControlValueAccessor {
  selected!: NgbDateStruct;
  disabled = false;
  onTouched: Function = () => {};
  onChange: Function = () => {};

  constructor(private changeDetector: ChangeDetectorRef) {}

  ngAfterViewInit() {
    this.changeDetector.detach();
  }
  writeValue(value: any): void {
    this.selected = value;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
  onChangeSelect(value: any) {
    this.selected = value;
    this.onChange(value);
  }
}
