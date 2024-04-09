import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SharedWorkerService {
  worker: SharedWorker;
  public shareTaskList = new Subject<{ id: number, value: string }[]>();

  constructor() {
    this.worker = new SharedWorker(
      new URL('../worker-shared.worker', import.meta.url)
    );

    this.worker.port.onmessage = ({ data }) => {
      this.shareTaskList.next(data);
    };
  }

  addTaskList(value: string) {
    this.worker.port.postMessage({type:'addTask', value});
  }

  deleteTaskList(id: number) {
    this.worker.port.postMessage({type:'deleteTask', value: id});
  }
}
