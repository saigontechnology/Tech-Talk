import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { SharedWorkerService } from '../../services/shared-worker.service';

@Component({
  selector: 'delete-task',
  templateUrl: './delete-task.component.html',
  styleUrls: ['./delete-task.component.scss']
})
export class DeleteTaskComponent implements OnInit {
  taskList: { id: number, value: string }[] = [];
  constructor(private sharedWorkerService: SharedWorkerService, private changeDetectorRef:ChangeDetectorRef) { }

  ngOnInit(): void {
    this.sharedWorkerService.addTaskList('');
    this.sharedWorkerService.shareTaskList.subscribe(taskListFromShareWorker => {
      this.taskList = taskListFromShareWorker;
      this.changeDetectorRef.detectChanges();
    })
  }

  deleteTask(id: number) {
    this.sharedWorkerService.deleteTaskList(id);
  }
}
