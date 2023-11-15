import { Component, HostBinding } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  animate,
  animateChild,
  animation,
  group,
  keyframes,
  query,
  state,
  style,
  transition,
  trigger,
  useAnimation,
} from '@angular/animations';
import { AnimationEvent } from '@angular/animations';

export const transitionAnimation = animation([
  style({
    height: '{{ height }}',
    opacity: '{{ opacity }}',
    backgroundColor: '{{ backgroundColor }}',
  }),
  animate('{{ time }}'),
]);

@Component({
  selector: 'app-open-close',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './open-close.component.html',
  styleUrls: ['./open-close.component.scss'],
  animations: [
    // animation triggers go here
    trigger('openClose', [
      state(
        'open',
        style({
          height: '200px',
          width: '300px',
          opacity: 1,
          backgroundColor: 'yellow',
        })
      ),
      state(
        'closed',
        style({
          height: '100px',
          width: '300px',
          opacity: 0.8,
          'background-color': 'green',
        })
      ),
      transition('open <=> closed', [animate('1s')]),
      transition('closed => open', [animate('0.5s')]),
    ]),
    trigger('multiStep', [
      transition('open => closed, closed => open', [
        animate(
          '3s',
          keyframes([
            style({
              opacity: 0,
              transform: 'scale(0.5)',
              offset: 0,
              color: 'red',
            }),
            style({
              opacity: 0.5,
              transform: 'scale(1.1)',
              offset: 0.3,
              color: 'blue',
            }),
            style({
              opacity: 1,
              transform: 'scale(1)',
              offset: 1,
              color: 'green',
            }),
          ])
        ),
      ]),
    ]),
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0 }), // Initial state (when appearing)
        animate('500ms', style({ opacity: 1 })), // Animation when appearing
      ]),
      transition(':leave', [
        animate('500ms', style({ opacity: 0 })), // Animation when disappearing
      ]),
    ]),
    trigger('fade', [
      transition('* => *', [
        style({ opacity: 0 }),
        animate('500ms', style({ opacity: 1, color: 'red' })),
      ]),
    ]),
    trigger('fadeInOutVoid', [
      transition('void => *', [
        // void state (element doesn't exist) to any state
        style({ opacity: 0 }), // Initial state (when appearing)
        animate('500ms', style({ opacity: 1, color: 'red' })), // Animation when appearing
      ]),
      transition('* => void', [
        // Any state to void state (element disappears)
        animate('500ms', style({ opacity: 0 })), // Animation when disappearing
      ]),
    ]),
    trigger('openCloseMulti', [
      state(
        'open',
        style({
          height: '200px',
          width: '500px',
          opacity: 1,
          backgroundColor: 'yellow',
        })
      ),
      state(
        'closed',
        style({
          // styles for closed
          height: '100px',
          width: '500px',
          opacity: 0.8,
          backgroundColor: 'blue',
        })
      ),
      state(
        'inProgress',
        style({
          // styles for inProgress
          height: '100px',
          width: '500px',
          opacity: 1,
          backgroundColor: 'orange',
        })
      ),
      transition('open => closed', [animate('1s')]),
      transition('closed => open', [animate('0.5s')]),
      transition('* => closed', [animate('1s')]),
      transition('* => open', [animate('0.5s'), style({ color: 'white' })]),
      transition('open <=> closed', [animate('0.5s')]),
      transition('* => open', [
        animate('4s', style({ color: 'red', opacity: '*' })),
      ]),
      transition('* => *', [animate('3s')]),
    ]),
    trigger('parentAnimation', [
      state(
        'expanded',
        style({ height: '*', border: '2px solid red', width: '300px' })
      ),
      state('collapsed', style({ height: '0', width: '300px' })),
      transition('expanded <=> collapsed', [
        animate('300ms'),
        query('@childAnimation', animateChild(), { optional: true }),
      ]),
    ]),
    trigger('childAnimation', [
      state('visible', style({ opacity: 1 })),
      state('hidden', style({ opacity: 0 })),
      transition('visible <=> hidden', animate('500ms')),
    ]),

    // Multiple animation triggers
    trigger('theParentAnimation', [
      state(
        'down',
        style({
          transform: 'translateY( 100% ) translateZ( 0 )',
        })
      ),
      state(
        'up',
        style({
          transform: 'translateY( 0% ) translateZ( 0 )',
        })
      ),
      transition('* <=> *', [
        group([
          query('@theChildAnimation', animateChild()),
          animate('0.9s cubic-bezier(0.55, 0.31, 0.15, 0.93)'),
        ]),
      ]),
    ]),
    trigger('theChildAnimation', [
      state(
        'down',
        style({
          transform: 'translateY( 100% ) translateZ( 0 )',
        })
      ),
      state(
        'up',
        style({
          transform: 'translateY( 0% ) translateZ( 0 )',
        })
      ),
      transition('* <=> *', [
        animate('0.9s cubic-bezier(0.55, 0.31, 0.15, 0.93)'),
      ]),
    ]),
    trigger('openClose', [
      transition('open => closed', [
        useAnimation(transitionAnimation, {
          params: {
            height: 0,
            opacity: 1,
            backgroundColor: 'red',
            time: '1s',
          },
        }),
      ]),
    ]),

    trigger('maskHover', [
      state(
        'void',
        style({
          '--mask-size': '150% 150%', // Initial state
        })
      ),
      state(
        'hover',
        style({
          '--mask-size': '0% 0%', // Hover state
        })
      ),
      transition('void => hover', animate('0.5s ease-in')),
      transition('hover => void', animate('0.5s ease-out')),
    ]),
  ],
})
export class OpenCloseComponent {
  // disabled all animations
  // @HostBinding('@.disabled')
  // animationsDisabled = true;

  isOpen = false;
  number = 0;

  parentState = 'expanded';

  items = [
    { name: 'Item 1', state: 'visible' },
    { name: 'Item 2', state: 'visible' },
    { name: 'Item 3', state: 'visible' },
  ];

  // -----------------------------
  state = 'up';
  toggleState() {
    this.state = this.state === 'up' ? 'down' : 'up';
  }

  onAnimationEvent(event: AnimationEvent) {
    // openClose is trigger name in this example
    console.log(`Animation Trigger: ${event.triggerName}`);

    // phaseName is "start" or "done"
    console.log(`Phase: ${event.phaseName}`);

    // in our example, totalTime is 1000 (number of milliseconds in a second)
    console.log(`Total time: ${event.totalTime}`);

    // in our example, fromState is either "open" or "closed"
    console.log(`From: ${event.fromState}`);

    // in our example, toState either "open" or "closed"
    console.log(`To: ${event.toState}`);
  }

  // ----------------------------------
}
