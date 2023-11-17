import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TestingHttpServiceComponent } from './testing-http-service.component';
import { TestingHttpServiceRoutingModule } from './testing-http-service-routing.module';

@NgModule({
  imports: [
    CommonModule,
    TestingHttpServiceRoutingModule
  ],
  declarations: [TestingHttpServiceComponent]
})
export class TestingHttpServiceModule { }
