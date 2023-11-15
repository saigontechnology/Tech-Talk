import {
  animate,
  keyframes,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, HostBinding, HostListener, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

export const jelloDiagonalAnimation = trigger('jelloDiagonalAnimation', [
  state('inactive', style({ transform: 'skew(0deg 0deg)' })),
  state('active', style({ transform: 'skew(0deg 0deg)' })),
  transition('inactive => active', [
    animate(
      '.9s',
      keyframes([
        style({ transform: 'skew(0deg, 0deg)', offset: 0 }),
        style({ transform: 'skew(25deg, 25deg)', offset: 0.3 }),
        style({ transform: 'skew(-15deg, -15deg)', offset: 0.4 }),
        style({ transform: 'skew(15deg, 15deg)', offset: 0.5 }),
        style({ transform: 'skew(-5deg, -5deg)', offset: 0.65 }),
        style({ transform: 'skew(5deg, 5deg)', offset: 0.75 }),
        style({ transform: 'skew(0deg, 0deg)', offset: 1 }),
      ])
    ),
  ]),
]);

@Component({
  selector: 'dialog-data',
  templateUrl: './dialog-data.component.html',
  styleUrls: ['./dialog-data.component.scss'],
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  animations: [
    jelloDiagonalAnimation,
    trigger('addItem', [
      state(
        '*',
        style({
          padding: '10px',
          margin: '10px',
          border: '1px dotted orange',
        })
      ),
      transition(':enter', [
        style({
          transform: 'translateX(-100%)',
        }),
        animate('0.5s ease-in'),
      ]),
      transition(':leave', [
        animate('0.5s ease-in', style({ transform: 'translateX(-100%)' })),
      ]),
    ]),
    trigger('maskHover', [
      state('void', style({})),
      state('hover', style({})),
      transition('void => hover', [
        animate(
          '0.4s',
          style({
            transform: 'scale(1) rotateZ(0)',
          })
        ),
        animate(
          '0.4s',
          style({
            transform: 'scale(.5) rotateZ(180deg)',
          })
        ),
        animate(
          '0.4s',
          style({
            transform: 'scale(1) rotateZ(360deg)',
          })
        ),
      ]),
      transition('hover => void', animate('0.5s ease-out')),
    ]),
  ],
})

// animate('.9s', keyframes([
//   style({ transform: 'skew(0deg 0deg)', offset: 0 }),
//   style({ transform: 'skew(25deg 25deg)', offset: 0.3 }),
//   style({ transform: 'skew(-15deg,-15deg)', offset: 0.4 }),
//   style({ transform: 'skew(15deg,15deg)', offset: 0.5 }),
//   style({ transform: 'skew(-5deg,-5deg)', offset: 0.65 }),
//   style({ transform: 'skew(5deg,5deg)', offset: 0.75 }),
//   style({ transform: 'skew(0deg 0deg)', offset: 1 })
// ]))
export class DialogDataDialog {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { disableAnimation: boolean }
  ) {}

  @HostBinding('@.disabled')
  animationsDisabled = this.data?.disableAnimation;

  items = [
    { name: 'Go To School', state: 'inactive' },
    { name: 'Buy Grocery', state: 'inactive' },
    { name: 'Workout 1 hour', state: 'inactive' },
  ];
  itemList: any[] = [];
  count = 0;

  animationState = 'inactive';
  animationRunning = false;

  addItem() {
    if (this.items.length > this.count) {
      this.itemList.push(this.items[this.count]);
      this.count++;
    }
  }

  removeItem() {
    if (this.count > 0) {
      this.itemList.pop();
      this.count--;
    }
  }

  toggleAnimation(item: any) {
    item.state = item.state === 'inactive' ? 'active' : 'inactive';
  }

  // -----------------------------

  hoverState: 'hover' | 'void' = 'void';

  @HostListener('mouseenter')
  onMouseEnter() {
    this.hoverState = 'hover';
  }

  @HostListener('mouseleave')
  onMouseLeave() {
    this.hoverState = 'void';
  }
}
