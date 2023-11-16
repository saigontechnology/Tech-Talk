import {
  AfterContentInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  DoCheck,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { CcomComponent } from '../ccom/ccom.component';

@Component({
  selector: 'app-bcom',
  templateUrl: './bcom.component.html',
  styleUrls: ['./bcom.component.scss'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CcomComponent],
})
export class BcomComponent
  implements OnChanges, OnInit, DoCheck, AfterContentInit
{
  @Input() value!: number[];

  constructor(public cdr: ChangeDetectorRef) {
    // cdr.detach();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // console.log('OnChanges B work!');
    // this.cdr.reattach();
    // setTimeout(() => {
    //   this.cdr.detach();
    // });
  }

  ngOnInit() {}

  ngDoCheck(): void {
    console.log('DoCheck B work!');
  }

  ngAfterContentInit(): void {}

  b(): void {}
}
