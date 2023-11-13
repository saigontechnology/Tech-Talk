import { ActivatedRoute } from '@angular/router';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FuelSalesService } from '../../services/fuel-sales.service';
import { map, Observable } from 'rxjs';
import { FUEL_SALES_FORM } from '../../models';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { CRUD_MODE, UNIT_TYPE } from '@sct-shared-lib';
import { formControlBase } from '@sct-shared-lib';
import { FormControlService } from '@sct-shared-lib';
import { ModalComponent } from '@sct-shared-lib';

@Component({
  selector: 'sct-fuel-sales-form',
  templateUrl: './fuel-sales-form.component.html',
  styleUrls: ['./fuel-sales-form.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuelSalesFormComponent implements OnInit {
  form!: FormGroup;
  isUpdateMode = false;

  unitType = UNIT_TYPE.LITRE.displayName;
  payLoad = '';

  changeUnitType = false;

  fuelTypeOptions$!: Observable<formControlBase<string>>;
  constructor(
    private _fuelSalesService: FuelSalesService,
    private _formControlService: FormControlService,
    private _activatedRoute: ActivatedRoute,
    private _modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this._activatedRoute.queryParams.subscribe((params: any) => {
      this.isUpdateMode = params.mode === CRUD_MODE.UPDATE;
    });
    this._initialSelect();
    this._initialForm();
  }

  onSubmit() {
    const modalRef = this._modalService.open(ModalComponent);

    this.payLoad = JSON.stringify(this.form.getRawValue());
    if(this.isUpdateMode){
      modalRef.componentInstance.modalMessage = 'You are about to update a fuel sale.';
      modalRef.componentInstance.yesText = 'Update';
      modalRef.componentInstance.modalHeader = 'Are you sure ?';
      modalRef.result.then((result) => {
        if (result === 'YES') {
          console.log(this.payLoad);
        }
      });
    }
    else{
      modalRef.componentInstance.modalMessage = 'Your New Fuel Sales successfully created';
      modalRef.componentInstance.showFullButtons = false;
    }
  }

  onDelete(){
    this.payLoad = JSON.stringify(this.form.getRawValue());
    const modalRef = this._modalService.open(ModalComponent);
    modalRef.componentInstance.modalMessage = 'You are about to delete a fuel sale.';
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
      tankID: new FormControl('', Validators.required),
      fuelType: new FormControl('', Validators.required),
      date: new FormControl('', Validators.required),
      fuelVolume: new FormControl('', [Validators.required]),
    });
    if (this.isUpdateMode) {
      this._fuelSalesService.selectedFuelSale$.subscribe((data) => {
        this.form.patchValue({
          tankID: data.id,
          fuelType: data.fuelType.id,
          date: { day: data.date.getDate(), month: data.date.getMonth() + 1, year: data.date.getFullYear() },
          fuelVolume: data.volume,
        });
      });
    }
  }

  get tankID() {
    return this.form.get('tankID');
  }
  get date() {
    return this.form.get('date');
  }
  get fuelType() {
    return this.form.get('fuelType');
  }
  get fuelVolume() {
    return this.form.get('fuelVolume');
  }

  private _initialSelect() {
    const rawData$ = this._getDataSelect();

    this.fuelTypeOptions$ = rawData$.pipe(
      map(({ fuelType }) => {
        return this._formControlService.formatDataForm(FUEL_SALES_FORM.FUEL_TYPE, fuelType);
      })
    );
  }

  private _getDataSelect() {
    return this._fuelSalesService.getFuelType().pipe(
      map((res) => {
        return {
          fuelType: res.content,
        };
      })
    );
  }
}
