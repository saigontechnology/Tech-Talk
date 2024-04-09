import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AppComponent } from "./app.component";
import { DedicatedWorkerComponent } from "./pages/dedicated-worker/dedicated-worker.component";
import { DeleteTaskComponent } from "./pages/delete-task/delete-task.component";
import { AddTaskComponent } from "./pages/add-task/add-task.component";


const routes: Routes = [
  {
    path: '',
    component: DedicatedWorkerComponent
  },
  {
    path: 'shared-worker/add-task',
    component: AddTaskComponent
  }, 
  {
    path: 'shared-worker/delete-task',
    component: DeleteTaskComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
