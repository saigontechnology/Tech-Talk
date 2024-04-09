import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routing';

import { DedicatedWorkerComponent } from './pages/dedicated-worker/dedicated-worker.component';
import { AddTaskComponent } from './pages/add-task/add-task.component';
import { DeleteTaskComponent } from './pages/delete-task/delete-task.component';

import { SharedWorkerService } from './services/shared-worker.service';

@NgModule({
  declarations: [
    AppComponent,
    DedicatedWorkerComponent,
    AddTaskComponent,
    DeleteTaskComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
  ],
  providers: [SharedWorkerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
