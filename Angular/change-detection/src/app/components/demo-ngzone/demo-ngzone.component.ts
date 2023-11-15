import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  DoCheck,
  NgZone,
  OnInit,
} from '@angular/core';
import { FcomComponent } from '../fcom/fcom.component';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-demo-ngzone',
  templateUrl: './demo-ngzone.component.html',
  standalone: true,
  // changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgIf],
})
export class DemoNgZoneComponent implements OnInit, DoCheck {
  progress: number = 0;
  label!: string;

  constructor(private _ngZone: NgZone) {}

  ngOnInit() {}

  ngDoCheck(): void {
    console.log('DoCheck NgZone work!');
  }


  processWithinAngularZone() {
    this.label = 'inside';
    this.progress = 0;
    this._increaseProgress(() => console.log('Inside Done!'));
  }

  // Loop outside of the Angular zone
  // so the UI DOES NOT refresh after each setTimeout cycle
  processOutsideOfAngularZone() {
    this.label = 'outside';
    this.progress = 0;
    this._ngZone.runOutsideAngular(() => {
      this._increaseProgress(() => {
        // reenter the Angular zone and display done
        this._ngZone.run(() => {
          console.log('Outside Done!');
        });
      });
    });
  }

  _increaseProgress(doneCallback: () => void) {
    this.progress += 1;
    console.log(`Current progress: ${this.progress}%`);

    if (this.progress < 100) {
      window.setTimeout(() => this._increaseProgress(doneCallback), 10);
    } else {
      doneCallback();
    }
  }
}
