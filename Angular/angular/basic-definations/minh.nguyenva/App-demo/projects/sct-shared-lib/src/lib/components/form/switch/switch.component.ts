import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ChangeDetectionStrategy, Component, Input, forwardRef } from '@angular/core';

@Component({
  selector: 'sct-switch',
  templateUrl: './switch.component.html',
  styleUrls: ['./switch.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SwitchComponent),
      multi: true,
    },
  ],
})
export class SwitchComponent implements ControlValueAccessor {
  @Input() switch!: any;

  selected = null;
  disabled = false;
  onChange: Function = () => {};
  onTouched: Function = () => {};

  constructor() {}

  
  writeValue(value: any): void {
    this.selected = value;
  }

  registerOnChange(fn: Function): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: Function): void {
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
