import { importProvidersFrom } from '@angular/core';
import { AppComponent } from './app/app.component';
import { routes } from './app/app-routing';
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
    provideRouter(routes),
    provideAnimations(),
    provideAnimations()
],
}).catch((err) => console.error(err));
