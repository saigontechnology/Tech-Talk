import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { A_ROUTING } from '@app/constants';

import { CommonService } from '../common.service';

@Component({
  selector: 'app-access-denied',
  templateUrl: './access-denied.component.html',
  styleUrls: ['./access-denied.component.scss']
})
export class AccessDeniedComponent implements OnInit {

  constructor(private _commonService: CommonService,
    private _router: Router) { }

  ngOnInit(): void {
    if (!this._commonService.accessDenied) {
      this._router.navigateByUrl(A_ROUTING.home);
      return;
    }
    this._commonService.accessDenied = false;
  }

}
