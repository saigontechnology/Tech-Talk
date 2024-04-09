import { CalculateHelper } from './../../helper/calculate.helper';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'dedicated-worker',
  templateUrl: './dedicated-worker.component.html',
  styleUrls: ['./dedicated-worker.component.scss'],
})
export class DedicatedWorkerComponent implements OnInit {
  worker!: Worker;
  dedicatedWorkerResult: { taskName: string; value: any }[] = [];
  noneDedicatedWorkerResult: { taskName: string; value: any }[] = [];
  number!: number;
  constructor() {}
  ngOnInit(): void {
    this.dedicatedWorkerInit();
  }

  dedicatedWorkerInit() {
    if (typeof Worker !== 'undefined') {
      this.worker = new Worker(new URL('../../app.worker', import.meta.url));
      
      // Listen for events using the addEventListener API
      // after calculate done, listen function here will render calculate value
      this.worker.addEventListener('message', ({ data }) => {
        this.dedicatedWorkerResult.push({
          taskName: 'Complex Task - Calculation Result:',
          value: data,
        });
      });

      this.worker.onerror = (error) => {
        this.dedicatedWorkerResult.push({
          taskName: 'Error From Dedicated Worker:',
          value: error,
        });
      };
    } else {
      // Web Workers are not supported in this environment.
      // You should add a fallback so that your program still executes correctly.
    }
  }

  runTask() {
    this.dedicatedWorkerResult = [];
    this.noneDedicatedWorkerResult = [];
    this.renderFormOneToNumberDontUseDedicatedWorker(
      this.number,
      `Simple Task: Render UI: `
    );
    this.renderFormOneToNumberUseDedicatedWorker(
      this.number,
      `Simple Task: Render UI: `
    );
  }

  async renderFormOneToNumberUseDedicatedWorker(
    number: number,
    taskName: string
  ) {

    // parallel calculate at Worker
    this.worker.postMessage({ eventName: 'calculate-complex-task', number });

    // and at the same time to render number
    for (let index = 1; index <= number; index++) {
      await CalculateHelper.customDelay(1000);
      this.dedicatedWorkerResult.push({ taskName: taskName, value: index });
    }
  }

  async renderFormOneToNumberDontUseDedicatedWorker(number: number, taskName: string) {
    // single thread, calculate first
    this.noneDedicatedWorkerResult.push({
      taskName: 'Complex Task - Calculation Result:',
      value: await CalculateHelper.calculateComplexTask(number),
    });

    // after calculate done, then render number
    for (let index = 1; index <= number; index++) {
      await CalculateHelper.customDelay(1000);
      this.noneDedicatedWorkerResult.push({ taskName: taskName, value: index });
    }
  }
}
