import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ROUTING, ROUTING_DATA } from './constants';

import { RoutingData } from '@cross/routing/models/routing-data.model';

import { AuthenticatedUserPolicy } from '@auth/policies/authenticated-user.policy';

import { HomePageComponent } from './home-page/home-page.component';
import { LayoutComponent } from './layout/layout.component';
import { CallbackComponent } from './oidc/callback/callback.component';
import { NotFoundComponent } from './common/not-found/not-found.component';
import { AccessDeniedComponent } from './common/access-denied/access-denied.component';
import { CreateResourceComponent } from './resource/create-resource/create-resource.component';
import { SilentRefreshComponent } from './oidc/silent-refresh/silent-refresh.component';

import { RoutingAuthService } from '@auth/routing/routing-auth.service';

const routes: Routes = [
  {
    path: ROUTING.base,
    component: LayoutComponent,
    children: [
      {
        path: ROUTING.home,
        component: HomePageComponent,
        data: {
          ...ROUTING_DATA.common,
          policies: [AuthenticatedUserPolicy]
        } as RoutingData,
        canActivate: [RoutingAuthService]
      },
      {
        path: ROUTING.resource.base,
        data: {
          ...ROUTING_DATA.common,
          policies: [AuthenticatedUserPolicy]
        } as RoutingData,
        canActivateChild: [RoutingAuthService],
        children: [
          {
            path: ROUTING.resource.create,
            component: CreateResourceComponent,
            data: {
              ...ROUTING_DATA.common
            } as RoutingData
          }
        ]
      },
      { path: ROUTING.notFound, component: NotFoundComponent },
      { path: ROUTING.accessDenied, component: AccessDeniedComponent }
    ]
  },
  { path: ROUTING.callback, component: CallbackComponent },
  { path: ROUTING.silentRefresh, component: SilentRefreshComponent },
  { path: '**', redirectTo: ROUTING.notFound }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
