import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TestingServiceComponent } from './testing-service.component';

const routes: Routes = [
  {
    path: '',
    component: TestingServiceComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TestingServiceRoutingModule { }
