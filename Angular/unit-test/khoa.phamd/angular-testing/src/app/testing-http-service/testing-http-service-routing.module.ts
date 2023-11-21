import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TestingHttpServiceComponent } from './testing-http-service.component';


const routes: Routes = [
  {
    path: '',
    component: TestingHttpServiceComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TestingHttpServiceRoutingModule { }