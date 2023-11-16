import { APP_INITIALIZER, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KeycloakService, KeycloakAngularModule } from 'keycloak-angular';
// import { initializeKeycloak } from '@core/keycloak/keycloak.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HeaderSetupInterceptor } from './interceptors';

const MODULES = [KeycloakAngularModule , HttpClientModule];

const interceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: HeaderSetupInterceptor, multi: true },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, ...MODULES],
  exports: [...MODULES],
  providers: [
    // {
    //   provide: APP_INITIALIZER,
    //   useFactory: initializeKeycloak,
    //   multi: true,
    //   deps: [KeycloakService],
    // },
    // ...interceptorProviders
  ],
})
export class CoreModule {}
