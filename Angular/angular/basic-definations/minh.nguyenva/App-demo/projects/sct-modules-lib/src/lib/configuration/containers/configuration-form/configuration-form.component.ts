import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { map, Observable } from 'rxjs';

import { ConfigurationService } from './../../services/configuration.service';
import { CONFIGURATION_FORM } from '../../model';

import { FormControlService } from '@sct-shared-lib';
import { formControlBase } from '@sct-shared-lib';import { UNIT_TYPE } from '@sct-shared-lib';

@Component({
  selector: 'sct-configuration-form',
  templateUrl: './configuration-form.component.html',
  styleUrls: ['./configuration-form.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfigurationFormComponent implements OnInit {
  form!: FormGroup;
  gasoline_unit$!: Observable<formControlBase<string>>;
  diesel_unit$!: Observable<formControlBase<string>>;

  constructor(private _formControlService: FormControlService, private _configService: ConfigurationService) {}

  ngOnInit(): void {
    this._initialForm();
    this._initialSelect();
  }
  onSubmit() {
    const payLoad = this.form.getRawValue();
    console.log(payLoad)

  }

  private _initialForm() {
    this.form = new FormGroup({
      gasolineThreshold: new FormControl('', Validators.required),
      gasolineUnit: new FormControl('', Validators.required),
      dieselThreshold: new FormControl('', Validators.required),
      dieselUnit: new FormControl('', Validators.required),
    });

    this.form.patchValue({
      gasolineUnit: UNIT_TYPE.LITRE.name,
      dieselUnit:  UNIT_TYPE.LITRE.name,
    })
  }

  get gasolineThreshold() {
    return this.form.get('gasolineThreshold');
  }
  get gasolineUnit() {
    return this.form.get('gasolineUnit');
  }
  get dieselThreshold() {
    return this.form.get('dieselThreshold');
  }
  get dieselUnit() {
    return this.form.get('dieselUnit');
  }


  
  private _initialSelect() {
    const rawData$ = this._getDataSelect();

    this.gasoline_unit$ = rawData$.pipe(
      map(({ fuelType }) => {
        return this._formControlService.formatDataForm(CONFIGURATION_FORM.GASOLINE_UNIT, fuelType);
      })
    );
    this.diesel_unit$ = rawData$.pipe(
      map(({ fuelType }) => {
        return this._formControlService.formatDataForm(CONFIGURATION_FORM.DIESEL_UNIT, fuelType);
      })
    );
  }

  
  private _getDataSelect() {
    return this._configService.getUnit().pipe(
      map((res) => {
        return {
          fuelType: res.content,
        };
      })
    );
  }
}
