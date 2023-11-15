import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  DoCheck,
  OnInit,
} from '@angular/core';
import { FcomComponent } from '../fcom/fcom.component';

@Component({
  selector: 'app-dcom',
  templateUrl: './dcom.component.html',
  styleUrls: ['./dcom.component.scss'],
  standalone: true,
  // changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FcomComponent],
})
export class DcomComponent implements OnInit, DoCheck {
  constructor(public cdr: ChangeDetectorRef) {}

  ngOnInit() {}

  ngDoCheck(): void {
    console.log('DoCheck D work!');
  }

  d(): void {}
}
