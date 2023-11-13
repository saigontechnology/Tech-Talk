import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { formControlBase, ControlType, filterInfo, options } from './../models/form-control-base';

@Injectable({
  providedIn: 'root',
})
export class FormControlService {
  constructor() {}

  toFormGroup(formGroup: formControlBase<string>[]) {
    const group: any = {};

    formGroup.forEach(({ key, validate, value }) => {
      group[key] = validate ? new FormControl(value || '', [...validate]) : new FormControl(value || '');
    });
    return new FormGroup(group);
  }

  formatDataForm(filterInfo: filterInfo, options: options[] | []): formControlBase<string> {
    return {
      key: filterInfo.name,
      label: filterInfo.displayName,
      controlType: filterInfo.controlType,
      options:
        filterInfo.controlType === ControlType.SELECT
          ? options.map((option) => {
              return {
                key: option.id,
                value: option.id,
                display: option.name,
              };
            })
          : [],
      iconClass: filterInfo?.iconClass,
      iconContent: filterInfo?.iconContent,
      validate: [],
    };
  }

  formatFilterData(filterInfo: filterInfo, options: options[] | [], validate?: Validators[]): formControlBase<string> {
    return {
      key: filterInfo.name,
      label: filterInfo.displayName,
      controlType: filterInfo.controlType,
      options:
        filterInfo.controlType === ControlType.SELECT
          ? options.map((option) => {
              return {
                key: option.id,
                value: option.id,
                display: option.name,
              };
            })
          : [],
      validate: validate,
    };
  }
}
