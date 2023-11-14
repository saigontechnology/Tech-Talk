import { Component, HostListener, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  animate,
  keyframes,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DialogDataDialog } from '../dialog-data/dialog-data.component';

export const scaleUpCenter = trigger('scaleUpCenter', [
  transition('* => *', [
    animate(
      '0.4s',
      keyframes([
        style({ transform: 'scale(0.5)', offset: 0 }),
        style({ transform: 'scale(1)', offset: 1 }),
      ])
    ),
  ]),
]);

export const wobbleVerticalLeft = trigger('wobbleVerticalLeft', [
  state(
    'default',
    style({
      transform: 'translateY(0) rotate(0)',
      transformOrigin: '50% 50%',
    })
  ),
  state(
    'wobble',
    style({
      transform: 'translateY(0) rotate(0)',
      transformOrigin: '50% 50%',
    })
  ),
  transition('default => wobble', [
    animate(
      '.8s',
      keyframes([
        style({
          transform: 'translateY(0) rotate(0)',
          transformOrigin: '50% 50%',
        }),
        style({
          transform: 'translateY(-30px) rotate(-6deg)',
          transformOrigin: '50% 50%',
        }),
        style({
          transform: 'translateY(15px) rotate(6deg)',
          transformOrigin: '50% 50%',
        }),
        style({
          transform: 'translateY(-15px) rotate(-3.6deg)',
          transformOrigin: '50% 50%',
        }),
        style({
          transform: 'translateY(9px) rotate(2.4deg)',
          transformOrigin: '50% 50%',
        }),
        style({
          transform: 'translateY(-6px) rotate(-1.2deg)',
          transformOrigin: '50% 50%',
        }),
      ])
    ),
  ]),
]);

interface SnowFlakeConfig {
  depth: number;
  left: number;
  speed: number;
}

@Component({
  selector: 'app-compare',
  standalone: true,
  imports: [
    CommonModule,
    MatMenuModule,
    MatButtonModule,
    MatCardModule,
    MatDialogModule,
  ],
  templateUrl: './compare.component.html',
  styleUrls: ['./compare.component.scss'],
  animations: [scaleUpCenter, wobbleVerticalLeft],
})
export class CompareComponent {
  dialog = inject(MatDialog);

  isOpen = false;
  infoList = [
    {
      state: 'default',
      title: 'Shiba Inu',
      subTitle: 'Dog Breed',
      descriptions:
        'The Shiba Inu is the smallest of the six original and distinct spitz breeds of dog from Japan.',
    },
    {
      state: 'default',
      title: 'Shiba Inu',
      subTitle: 'Dog Breed',
      descriptions:
        'The Shiba Inu is the smallest of the six original and distinct spitz breeds of dog from Japan.',
    },
    {
      state: 'default',
      title: 'Shiba Inu',
      subTitle: 'Dog Breed',
      descriptions:
        'The Shiba Inu is the smallest of the six original and distinct spitz breeds of dog from Japan.',
    },
    {
      state: 'default',
      title: 'Shiba Inu',
      subTitle: 'Dog Breed',
      descriptions:
        'The Shiba Inu is the smallest of the six original and distinct spitz breeds of dog from Japan.',
    },
    {
      state: 'default',
      title: 'Shiba Inu',
      subTitle: 'Dog Breed',
      descriptions:
        'The Shiba Inu is the smallest of the six original and distinct spitz breeds of dog from Japan.',
    },
    {
      state: 'default',
      title: 'Shiba Inu',
      subTitle: 'Dog Breed',
      descriptions:
        'The Shiba Inu is the smallest of the six original and distinct spitz breeds of dog from Japan.',
    },
    {
      state: 'default',
      title: 'Shiba Inu',
      subTitle: 'Dog Breed',
      descriptions:
        'The Shiba Inu is the smallest of the six original and distinct spitz breeds of dog from Japan.',
    },
  ];

  snowFlakes!: SnowFlakeConfig[];

  @HostListener('mouseenter')
  onMouseEnter(item: any) {
    if (item) {
      item.state = 'wobble';
    }
  }

  @HostListener('mouseleave')
  onMouseLeave(item: any) {
    if (item) {
      item.state = 'default';
    }
  }

  constructor() {}

  openDialog(disableAnimation: boolean) {
    this.dialog.open(DialogDataDialog, {
      data: {
        disableAnimation,
      },
    });
  }
}
