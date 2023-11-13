import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KeycloakService } from 'keycloak-angular';
import { environment } from '@environments/environment';

// export function initializeKeycloak(keycloak: KeycloakService): () => Promise<any> {
//   return () =>
//     keycloak.init({
//       config: {
//         url: environment.appConfig.keyCloakUrl,
//         realm: environment.realm,
//         clientId: 'web-client',
//       },
//       initOptions: {
//         onLoad: 'login-required',
//         checkLoginIframe: true,
//       },
//       loadUserProfileAtStartUp: true,
//       bearerExcludedUrls: ['/assets', '/clients/public'],
//     });
// }

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class KeycloakModule {}
