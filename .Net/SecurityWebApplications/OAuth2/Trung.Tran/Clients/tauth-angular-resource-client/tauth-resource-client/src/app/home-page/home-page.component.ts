import { Component, OnInit } from '@angular/core';

import { HttpHelperService } from '@cross/http/http-helper.service';

import { A_ROUTING } from '@app/constants';

import { ResourceListItemModel } from '@app/resource/models/resource-list-item.model';

import { ResourceService } from '@app/resource/resource.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {

  A_ROUTING = A_ROUTING;

  resourceList: ResourceListItemModel[];

  constructor(private _resourceService: ResourceService,
    private _httpHelper: HttpHelperService) {
    this.resourceList = [];
  }

  ngOnInit(): void {
    this._resourceService.getResourceList().subscribe(resourceList => {
      this.resourceList = resourceList;
    }, err => this._httpHelper.handleCommonError(err));
  }

  onDeleteClicked(deletedResource: ResourceListItemModel) {
    this._resourceService.deleteResource(deletedResource.id).subscribe(() => {
      this.resourceList.splice(this.resourceList.findIndex(resource => resource === deletedResource), 1);
    }, err => this._httpHelper.handleCommonError(err));
  }
}
