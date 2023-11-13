import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'sct-circle-progress',
  templateUrl: './circle-progress.component.html',
  styleUrls: ['./circle-progress.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CircleProgressComponent implements OnInit {
  @Input() progressName!: string ;
  @Input() progressIcon!: string ;
  @Input() progressColor!: string ;
  @Input() progressData!:any;
  
  constructor() {}

  ngOnInit(): void {}
}
