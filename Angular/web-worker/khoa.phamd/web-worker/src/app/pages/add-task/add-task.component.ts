import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { SharedWorkerService } from '../../services/shared-worker.service';

@Component({
  selector: 'add-task',
  templateUrl: './add-task.component.html',
  styleUrls: ['./add-task.component.scss']
})
export class AddTaskComponent implements OnInit {
  task: string = '';
  taskList: { id: number, value: string }[] = [];
  constructor(private sharedWorkerService: SharedWorkerService, private changeDetectorRef:ChangeDetectorRef) { }

  ngOnInit(): void {
    this.sharedWorkerService.shareTaskList.subscribe(taskListFromShareWorker => {
      this.taskList = taskListFromShareWorker;
      this.changeDetectorRef.detectChanges();
    })
  }

  addTask() {
    this.sharedWorkerService.addTaskList(this.task);
  }

  navigateToDeletePage() {
    window.open('/shared-worker/delete-task','_blank');
  }
}
