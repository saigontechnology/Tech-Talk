import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { combineLatest, map, Observable } from 'rxjs';

import { ImportService } from '../../services/import.service';
import { IMPORT_FORM } from '../../model';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Coordinates } from '@sct-shared-lib';
import { MapMode } from '@sct-shared-lib';
import { CRUD_MODE, UNIT_TYPE } from '@sct-shared-lib';
import { formControlBase } from '@sct-shared-lib';import { FormControlService } from '@sct-shared-lib';
import { ModalComponent } from '@sct-shared-lib';

@Component({
  selector: 'sct-import-form',
  templateUrl: './import-form.component.html',
  styleUrls: ['./import-form.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ImportFormComponent implements OnInit {
  form!: FormGroup;
  isUpdateMode = false;

  MapMode = MapMode;

  unitType = UNIT_TYPE.LITRE.displayName;
  payLoad = '';

  changeUnitType = false;

  fuelTypeOptions$!: Observable<formControlBase<string>>;
  countryOption$!: Observable<formControlBase<string>>;
  centerCoordinates!: Coordinates;

  constructor(
    private _importService: ImportService,
    private _formControlService: FormControlService,
    private _modalService: NgbModal,
    private _activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this._activatedRoute.queryParams.subscribe((params: any) => {
      this.isUpdateMode = params.mode === CRUD_MODE.UPDATE;
    });
    this._initialForm();
    this._initialSelect();
  }

  onSubmit() {
    const modalRef = this._modalService.open(ModalComponent);

    this.payLoad = JSON.stringify(this.form.getRawValue());
    if (this.isUpdateMode) {
      modalRef.componentInstance.modalMessage = 'You are about to update a import.';
      modalRef.componentInstance.yesText = 'Update';
      modalRef.componentInstance.modalHeader = 'Are you sure ?';
      modalRef.result.then((result) => {
        if (result === 'YES') {
          console.log(this.payLoad);
        }
      });
    } else {
      modalRef.componentInstance.modalMessage = 'Your new import successfully created';
      modalRef.componentInstance.showFullButtons = false;
    }
  }

  onDelete() {
    this.payLoad = JSON.stringify(this.form.getRawValue());
    const modalRef = this._modalService.open(ModalComponent);
    modalRef.componentInstance.modalMessage = 'You are about to delete a import';
    modalRef.componentInstance.yesText = 'Delete';
    modalRef.componentInstance.modalHeader = 'Are you sure ?';
    modalRef.result.then((result) => {
      if (result === 'YES') {
        console.log(this.payLoad);
      }
    });
  }

  changeVoulumeType() {
    this.changeUnitType = !this.changeUnitType;
    if (this.changeUnitType) {
      this.unitType = UNIT_TYPE.CUBE_METRE.displayName;
    } else {
      this.unitType = UNIT_TYPE.LITRE.displayName;
    }
  }

  private _initialForm() {
    this.form = new FormGroup({
      importPoint: new FormControl('', Validators.required),
      fuelType: new FormControl('', Validators.required),
      date: new FormControl('', Validators.required),
      countryOfOrigin: new FormControl('', Validators.required),
      fuelVolume: new FormControl('', [Validators.required]),
    });
    if (this.isUpdateMode) {
      this._importService.selectedImport$.subscribe((data) => {
        this.form.patchValue({
          countryOfOrigin: data.countryOfOrigin.name,
          importPoint: data.importPoint.station,
          fuelType: data.fuelType.id,
          date: { day: data.date.getDate(), month: data.date.getMonth() + 1, year: data.date.getFullYear() },
          fuelVolume: data.volume,
        });
      });
    }

    this.countryOfOrigin?.valueChanges.subscribe(data=>{console.log(data)})
  }

  get countryOfOrigin() {
    return this.form.get('countryOfOrigin');
  }
  get fuelType() {
    return this.form.get('fuelType');
  }
  get fuelVolume() {
    return this.form.get('fuelVolume');
  }
  get importPoint() {
    return this.form.get('importPoint');
  }
  get date() {
    return this.form.get('date');
  }

  private _initialSelect() {
    const rawData$ = this._getDataSelect();

    this.fuelTypeOptions$ = rawData$.pipe(
      map(({ fuelType }) => {
        return this._formControlService.formatDataForm(IMPORT_FORM.FUEL_TYPE, fuelType);
      })
    );
    this.countryOption$ = rawData$.pipe(
      map(({ countryOfOrigin }) => {
        return this._formControlService.formatDataForm(IMPORT_FORM.COUNTRY_OF_ORIGIN, countryOfOrigin);
      })
    );
  }

  private _getDataSelect() {
    return combineLatest({
      countryOfOrigin: this._importService.getCountryofOrigin(),
      fuelType: this._importService.getFuelType(),
    }).pipe(
      map(({ countryOfOrigin, fuelType }) => {
        return {
          fuelType: fuelType.content,
          countryOfOrigin: countryOfOrigin.content,
        };
      })
    );
  }
}
