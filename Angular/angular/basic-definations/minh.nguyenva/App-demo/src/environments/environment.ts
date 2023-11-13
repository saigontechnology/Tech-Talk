// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  realm: 'fuel-integrity-solution',
  production: false,
  appConfig: {
    apiUrl: 'http://app-sct-dev.ogptsqa.dev.sicpa.io/api',
    keyCloakUrl: 'http://app-sct-dev.ogptsqa.dev.sicpa.io/auth/',
    defaultCoordinates: {
      latitude: 4.621116,
      longitude: 118.534400,
    },
  },
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
