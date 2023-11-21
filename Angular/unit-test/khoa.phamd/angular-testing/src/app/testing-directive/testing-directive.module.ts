import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TestingDirectiveComponent } from './testing-directive.component';
import { TestingDirectiveRoutingModule } from './testing-directive-routing.module';
import { CopyrightDirective } from './copyright.directive';

@NgModule({
  imports: [
    CommonModule,
    TestingDirectiveRoutingModule
  ],
  declarations: [TestingDirectiveComponent, CopyrightDirective]
})
export class TestingDirectiveModule { }
