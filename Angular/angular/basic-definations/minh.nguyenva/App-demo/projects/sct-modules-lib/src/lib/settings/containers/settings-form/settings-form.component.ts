import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { map, Observable, combineLatest } from 'rxjs';

import { SETTING_FORM, SWICHES } from '../../models';
import { SettingsService } from './../../services/settings.service';

import { formControlBase } from '@sct-shared-lib';import { FormControlService } from '@sct-shared-lib';

@Component({
  selector: 'sct-settings-form',
  templateUrl: './settings-form.component.html',
  styleUrls: ['./settings-form.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SettingsFormComponent implements OnInit {
  form!: FormGroup;
  SWICHES = SWICHES;
  countryOption$!: Observable<formControlBase<string>>;

  constructor(private _settingsService: SettingsService, private _formControlService: FormControlService) {}

  ngOnInit(): void {
    this._initialForm();
    this._initialSelect();
  }

  onSubmit() {
    const payLoad = this.form.getRawValue();
    console.log(payLoad);
  }
  private _initialForm() {
    this.form = new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', Validators.required),
      mobileNumber: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      currentPassword: new FormControl('', Validators.required),
      newPassWord: new FormControl('', Validators.required),
      newPassWordConfirm: new FormControl('', Validators.required),
      deliveryStatus: new FormControl('', Validators.required),
      emailNotification: new FormControl('', Validators.required),
      deliveryStatus1: new FormControl('', Validators.required),
      deliveryStatus2: new FormControl('', Validators.required),
      deliveryStatus3: new FormControl('', Validators.required),
    });
  }

  get firstName() {
    return this.form.get('firstName');
  }
  get lastName() {
    return this.form.get('lastName');
  }
  get email() {
    return this.form.get('email');
  }
  get mobileNumber() {
    return this.form.get('mobileNumber');
  }
  get country() {
    return this.form.get('country');
  }
  get currentPassword() {
    return this.form.get('currentPassword');
  }
  get newPassWord() {
    return this.form.get('newPassWord');
  }
  get newPassWordConfirm() {
    return this.form.get('newPassWordConfirm');
  }

  private _initialSelect() {
    const rawData$ = this._getDataSelect();

    this.countryOption$ = rawData$.pipe(
      map(({country }) => {
        return this._formControlService.formatDataForm(SETTING_FORM.COUNTRY,country);
      })
    );
  }

  private _getDataSelect() {
    return this._settingsService.getCountry().pipe(
      map((data) => {
        return {
          country: data.content,
        };
      })
    );
  }
}
