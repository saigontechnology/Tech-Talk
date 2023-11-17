import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TestingDirectiveComponent } from './testing-directive.component';

const routes: Routes = [
  {
    path: '',
    component: TestingDirectiveComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TestingDirectiveRoutingModule { }
