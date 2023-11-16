import {
  ChangeDetectionStrategy,
  Component,
  DoCheck,
  OnInit,
} from '@angular/core';

@Component({
  selector: 'app-ccom',
  templateUrl: './ccom.component.html',
  styleUrls: ['./ccom.component.scss'],
  standalone: true,
  // changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CcomComponent implements OnInit, DoCheck {
  constructor() {}

  ngOnInit() {}

  ngDoCheck(): void {
    console.log('DoCheck C work!');
  }

  c(): void {}
}
