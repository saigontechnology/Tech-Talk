import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TestingDependencyServiceComponent } from './testing-dependency-service.component';


const routes: Routes = [
  {
    path: '',
    component: TestingDependencyServiceComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TestingDependencyServiceRoutingModule { }