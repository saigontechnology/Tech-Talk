import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  DoCheck,
  ElementRef,
  NgZone,
  OnInit,
  ViewChild,
  inject,
} from '@angular/core';
import { BcomComponent } from '../bcom/bcom.component';
import { fromEvent } from 'rxjs';

@Component({
  selector: 'app-acom',
  templateUrl: './acom.component.html',
  styleUrls: ['./acom.component.scss'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [BcomComponent],
})
export class AcomComponent implements OnInit, DoCheck, AfterViewInit {
  @ViewChild('aButton') aButton!: ElementRef;

  #ngZone = inject(NgZone);

  public value = [10];

  constructor(public cdr: ChangeDetectorRef) {
    // cdr.detach();
    // setInterval(() => {
    //   this.cdr.detectChanges();
    // }, 5000);
  }

  ngOnInit() {}

  ngDoCheck(): void {
    console.log('DoCheck A work!');
    // this.value.push(20);
    // this.cdr.markForCheck();
  }

  ngAfterViewInit(): void {
    console.log('AfterViewInit A work!');
    // this.value.push(20);
    // this.value = [10, 20];
    // this.cdr.reattach();
    // this.cdr.markForCheck();
    // this.cdr.detectChanges();

    // this.cdr.detectChanges();
    // this.cdr.detach();

    // setTimeout(() => {
    //  this.cdr.reattach(); 
    // });
  }

  a(): void {}

  divClick(): void {}
}
