import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {HttpClientModule} from "@angular/common/http";
import {CoursesModule} from './courses/courses.module';
import { FirstDemoComponent } from './first-demo/first-demo.component';
import { TestingServiceModule } from './testing-service/testing-service.module';
import { MatComponentsModule } from './mat-components.module';

@NgModule({
    declarations: [
      AppComponent,
      FirstDemoComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MatComponentsModule,
        HttpClientModule,
        CoursesModule,
        AppRoutingModule,
        TestingServiceModule
    ],
    providers: [
    ],
    bootstrap: [AppComponent]

})
export class AppModule {
}
