import { ChangeDetectionStrategy, Component, OnInit, Input } from '@angular/core';
import { LEVEL_PROGRESS_DATASETS, LEVEL_PROGRESS_NAME } from './level-progress.model';

@Component({
  selector: 'sct-level-progress',
  templateUrl: './level-progress.component.html',
  styleUrls: ['./level-progress.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LevelProgressComponent implements OnInit {
  @Input() progressName!:string;
  @Input() progressData!: LEVEL_PROGRESS_DATASETS;


  constructor() { }

  ngOnInit(): void {
  }

}
