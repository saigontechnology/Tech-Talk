import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TestingServiceComponent } from './testing-service.component';
import { TestingServiceRoutingModule } from './testing-service-routing.module';
import { MatComponentsModule } from '../mat-components.module';

@NgModule({
  imports: [
    CommonModule,
    MatComponentsModule,
    TestingServiceRoutingModule
  ],
  declarations: [
    TestingServiceComponent,
  ]
})
export class TestingServiceModule { }
