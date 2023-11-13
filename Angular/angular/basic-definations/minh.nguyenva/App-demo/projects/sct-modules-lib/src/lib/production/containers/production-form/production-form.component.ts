import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { map, Observable, Subject, takeUntil } from 'rxjs';
import { filter, startWith, switchMap, tap } from 'rxjs/operators';

import { ActivatedRoute } from '@angular/router';

import { ProductionListResponse, PRODUCTION_FORM } from '../../model';
import { ProductionService } from '../../services/production.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import {
  Coordinates,
  MapMode,
  CRUD_MODE,
  formControlBase,
  FormControlService,
  DateValidator,
  ModalComponent,
  LoaderService,
  NavigationService,
  MODAL_STATUS,
  SITE_TYPE,
  LocationSiteResponse,
} from '@sct-shared-lib';

import { CoreService } from '@core/services';

@Component({
  selector: 'sct-production-form',
  templateUrl: './production-form.component.html',
  styleUrls: ['./production-form.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductionFormComponent implements OnInit {
  form!: FormGroup;
  isUpdateMode = false;

  selectedProduction!: ProductionListResponse;
  MapMode = MapMode;

  fuelTypeOptions$!: Observable<formControlBase<string>>;
  pointOfOrigin$!: Observable<formControlBase<string>>;

  productionPointOption$!: Observable<formControlBase<string>>;
  centerCoordinates: Coordinates = {
    latitude: 0,
    longitude: 0,
  };
  currentCountryId!: string;
  currentSiteId = '';

  private readonly _destroy$ = new Subject<void>();

  constructor(
    private _productionSerivce: ProductionService,
    private _formControlService: FormControlService,
    private _activatedRoute: ActivatedRoute,
    private _modalService: NgbModal,
    private _coreService: CoreService,
    private _loaderService: LoaderService,
    private _navigationService: NavigationService,
    private _cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this._activatedRoute.queryParams.subscribe((params: any) => {
      this.isUpdateMode = params.mode === CRUD_MODE.UPDATE;
    });
    this._initialForm();
    this._initialSelect();
  }

  onSubmit() {
    const importRequestBody = this._mapParams(this.form.getRawValue());

    if (this.isUpdateMode) {
      const modalRef = this._modalService.open(ModalComponent);
      modalRef.componentInstance.modalMessage = 'You are about to update a production.';
      modalRef.componentInstance.yesText = 'Update';
      modalRef.componentInstance.modalHeader = 'Are you sure ?';
      modalRef.result.then((result) => {
        if (result === MODAL_STATUS.YES) {
          this._productionSerivce
            .updateProduction(this.selectedProduction.id, importRequestBody)
            .pipe(takeUntil(this._destroy$))
            .subscribe(
              (res) => {
                const updateModal = this._showMessageModal('Your new production updated');
                updateModal.result.then((result) => {});
              },
              (err) => {
                this._showMessageModal(`Updated failed: ${err.error.errors} `);
              }
            );
        }
      });
    } else {
      this._productionSerivce
        .createProduction(importRequestBody)
        .pipe(takeUntil(this._destroy$))
        .subscribe(
          (res) => {
            const createModal = this._showMessageModal('Your new production successfully created');
            createModal.result.then((result) => {
              this._navigationService.back();
            });
          },
          (err) => {
            this._showMessageModal(`Created failed: ${err.error.errors}`);
          }
        );
    }
  }
  onDelete() {
    const modalRef = this._modalService.open(ModalComponent);
    modalRef.componentInstance.modalMessage = 'You are about to delete a production';
    modalRef.componentInstance.yesText = 'Delete';
    modalRef.componentInstance.modalHeader = 'Are you sure ?';
    modalRef.result.then((result) => {
      if (result === MODAL_STATUS.YES) {
        this._productionSerivce.deleteProduction(this.selectedProduction.id).subscribe(
          (res) => {
            const deleteModal = this._showMessageModal('Production deleted');
            deleteModal.result.then((result) => {
              this._navigationService.back();
            });
          },
          (err) => {
            this._showMessageModal(`Deleted failed: ${err.error.errors}`);
          }
        );
      }
    });
  }

  private _initialForm() {
    this.form = new FormGroup({
      fuelType: new FormControl('', Validators.required),
      productionPoint: new FormControl('', Validators.required),
      date: new FormControl('', [Validators.required, DateValidator.FULL_DATE]),
      fuelVolume: new FormControl('', [Validators.required, Validators.min(0)]),
    });
    if (this.isUpdateMode) {
      this._getProductionById();
    }
  }

  get productionPoint() {
    return this.form.get('productionPoint');
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
    this.productionPointOption$ = this._coreService.locationSite$.pipe(
      map((productionPoint) => {
        const productionPointFilterd = productionPoint.filter((location: LocationSiteResponse) => location.typeCode === SITE_TYPE.REFINERIES.typeCode);
        this.currentCountryId = productionPointFilterd[0].country.id;
        return this._formControlService.formatDataForm(PRODUCTION_FORM.PRODUCTION_POINT, productionPointFilterd);
      })
    );

    this.fuelTypeOptions$ = this._coreService.fuelTypes$.pipe(
      map((fuelType) => {
        return this._formControlService.formatDataForm(PRODUCTION_FORM.FUEL_TYPE, fuelType);
      })
    );

    this.productionPoint?.valueChanges
      .pipe(
        takeUntil(this._destroy$),
        startWith(this.productionPoint.value),
        filter((productionPoint) => !!productionPoint),
        tap((productionPoint) => (this.currentSiteId = productionPoint)),
        switchMap((productionPoint) => {
          return this._coreService.locationSite$;
        })
      )
      .subscribe((productionList: any[]) => {
        const { coordinates } = productionList.find((productionPoint) => productionPoint.id === this.currentSiteId);
        const location = {
          latitude: coordinates.y,
          longitude: coordinates.x,
        };
        this.centerCoordinates = { ...location };
      });
  }

  private _getProductionById() {
    this._loaderService.showLoader();
    this._productionSerivce.selectedProduction$.subscribe(({ selectedProduction, siteId }) => {
      this._loaderService.hideLoader();
      const requestedDate = new Date(selectedProduction.requestedDate);
      this.selectedProduction = selectedProduction;
      this.form.patchValue({
        productionPoint: siteId,
        fuelType: selectedProduction.fuelType.id,
        date: { day: requestedDate.getDate(), month: requestedDate.getMonth() + 1, year: requestedDate.getFullYear() },
        fuelVolume: selectedProduction.volume.value,
      });
      this._cd.markForCheck();
      this._cd.detectChanges();
    });
  }

  private _showMessageModal(message: string, isShowFullButton = false) {
    const modalRef = this._modalService.open(ModalComponent);
    modalRef.componentInstance.modalMessage = message;
    modalRef.componentInstance.showFullButtons = isShowFullButton;
    return modalRef;
  }

  private _mapParams(rawParmas: any) {
    return {
      siteId: rawParmas.productionPoint.toString(),
      requestedDate: new Date(rawParmas.date.year, rawParmas.date.month, rawParmas.date.day).toISOString(),
      fuelTypeId: rawParmas.fuelType.toString(),
      volume: {
        scale: 'ABSOLUTE',
        value: parseFloat(rawParmas.fuelVolume),
        unit: 'L',
      },
    };
  }
}
