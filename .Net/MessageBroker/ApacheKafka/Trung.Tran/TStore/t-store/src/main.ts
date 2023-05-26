import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';

import { dynamicUrls } from './app/constants/urls.const';
import { environment } from './environments/environment';

import { getUrls } from './app/helpers/url.helper';

if (environment.production) {
  enableProdMode();
}

(function configure() {
  getUrls().then(urls => {
    Object.assign(dynamicUrls, urls);
    platformBrowserDynamic().bootstrapModule(AppModule)
      .catch(err => console.error(err));
  });
}());