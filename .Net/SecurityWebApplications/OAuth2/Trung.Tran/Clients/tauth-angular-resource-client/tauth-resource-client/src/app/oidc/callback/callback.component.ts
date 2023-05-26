import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { A_ROUTING } from '@app/constants';

import { IdentityService } from '@auth/identity/identity.service';
import { UserService } from '@app/user/user.service';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {

  constructor(private _identityService: IdentityService,
    private _userService: UserService,
    private _router: Router) { }

  ngOnInit(): void {
    this._identityService.signinRedirectCallback()
      .then(async user => {
        const profileItems = await this._userService.getUserProfile().toPromise();
        profileItems.forEach(profileItem => user.profile[profileItem.type] = profileItem.value);
        this._identityService.storeUser(user);
        this._router.navigateByUrl(user.state?.returnUrl || A_ROUTING.home);
      })
      .catch(err => console.log(err));
  }

}
