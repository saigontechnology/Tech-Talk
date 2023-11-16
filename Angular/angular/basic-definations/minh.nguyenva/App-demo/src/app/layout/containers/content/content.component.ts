import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { LoaderService } from '@sct-shared-lib';

@Component({
  selector: 'sct-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContentComponent implements OnInit {
  sideNavExpanded = true;
  isShowLoader = false;

  constructor(private _loaderService: LoaderService) {}

  ngOnInit(): void {
    this._loaderService.loader$.subscribe((isLoad: boolean) => {
      this.isShowLoader = isLoad ? isLoad : false;
    });
  }
}
