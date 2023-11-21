import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TestingDependencyServiceComponent } from './testing-dependency-service.component';
import { TestingDependencyServiceRoutingModule } from './testing-dependency-service-routing.module';

@NgModule({
  imports: [
    CommonModule,
    TestingDependencyServiceRoutingModule
  ],
  declarations: [TestingDependencyServiceComponent]
})
export class TestingDependencyServiceModule { }
