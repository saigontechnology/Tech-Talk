import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HTTP_INTERCEPTORS_PROVIDERS } from '@cross/http/interceptors/constants';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from './home-page/home-page.component';
import { CallbackComponent } from './oidc/callback/callback.component';
import { LayoutComponent } from './layout/layout.component';
import { AuthModule } from '@auth/auth.module';
import { NotFoundComponent } from './common/not-found/not-found.component';
import { AccessDeniedComponent } from './common/access-denied/access-denied.component';
import { CreateResourceComponent } from './resource/create-resource/create-resource.component';
import { SilentRefreshComponent } from './oidc/silent-refresh/silent-refresh.component';

@NgModule({
  declarations: [
    AppComponent,
    HomePageComponent,
    CallbackComponent,
    LayoutComponent,
    NotFoundComponent,
    AccessDeniedComponent,
    CreateResourceComponent,
    SilentRefreshComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AuthModule
  ],
  providers: [
    HTTP_INTERCEPTORS_PROVIDERS
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
