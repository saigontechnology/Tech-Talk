import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TestingPipeComponent } from './testing-pipe.component';
import { TestingPipeRoutingModule } from './testing-pipe-routing.module';

@NgModule({
  imports: [
    CommonModule,
    TestingPipeRoutingModule
  ],
  declarations: [TestingPipeComponent]
})
export class TestingPipeModule { }
