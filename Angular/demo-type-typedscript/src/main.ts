import { importProvidersFrom } from '@angular/core';
import { AppComponent } from './app/app.component';

import { BrowserModule, bootstrapApplication } from '@angular/platform-browser';
import {
  BrowserAnimationsModule,
  provideAnimations,
} from '@angular/platform-browser/animations';
import { RouterModule, provideRouter } from '@angular/router';

bootstrapApplication(AppComponent, {
  providers: [
    provideAnimations(),
    importProvidersFrom(BrowserModule, BrowserAnimationsModule),
  ],
}).catch((err) => console.error(err));
