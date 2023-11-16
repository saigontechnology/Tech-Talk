import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppComponent } from './app/app.component';
import { bootstrapApplication } from '@angular/platform-browser';
import { NgZone, provideZoneChangeDetection } from '@angular/core';

bootstrapApplication(AppComponent, {
  providers: [provideZoneChangeDetection({ eventCoalescing: true })],
}).catch((err) => console.error(err));
