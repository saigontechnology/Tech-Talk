import {
  ChangeDetectionStrategy,
  Component,
  DoCheck,
  OnInit,
} from '@angular/core';

@Component({
  selector: 'app-fcom',
  templateUrl: './fcom.component.html',
  styleUrls: ['./fcom.component.scss'],
  standalone: true,
  // changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FcomComponent implements OnInit, DoCheck {
  constructor() {}

  ngOnInit() {}

  ngDoCheck(): void {
    console.log('DoCheck F work!');
  }

  f(): void {}
}
