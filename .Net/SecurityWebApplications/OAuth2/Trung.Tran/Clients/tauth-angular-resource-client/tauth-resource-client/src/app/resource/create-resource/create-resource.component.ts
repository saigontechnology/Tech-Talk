import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { A_ROUTING } from '@app/constants';

import { FormHelper } from '@cross/form/form-helper';

import { CreateResourceModel } from '../models/create-resource-model';

import { HttpHelperService } from '@cross/http/http-helper.service';
import { ResourceService } from '../resource.service';

@Component({
  selector: 'app-create-resource',
  templateUrl: './create-resource.component.html',
  styleUrls: ['./create-resource.component.scss']
})
export class CreateResourceComponent implements OnInit {

  resourceFormGroup!: FormGroup;

  constructor(private _formBuilder: FormBuilder,
    private _resourceService: ResourceService,
    private _httpHelper: HttpHelperService,
    private _router: Router) { }

  ngOnInit(): void {
    this.resourceFormGroup = this._formBuilder.group({
      name: ['', [Validators.required]]
    });
  }

  onFormSubmit(): void {
    const isValid = FormHelper.validateFormGroup(this.resourceFormGroup);
    if (!isValid) return;
    const createResourceModel = this.resourceFormGroup.value as CreateResourceModel;
    this._resourceService.createResource(createResourceModel)
      .subscribe(id => this._router.navigateByUrl(A_ROUTING.home), err => this._httpHelper.handleCommonError(err))
  }
}
