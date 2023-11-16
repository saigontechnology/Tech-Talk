import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { startWith, takeUntil } from 'rxjs/operators';
import { Breadcrumb } from './breadcrumb.model';
import { BreadcrumbService } from './breadcrumb.service';

@Component({
  selector: 'sct-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BreadcrumbComponent implements OnInit {
  @Input() showHome!: boolean;
  breadcrumbs$!: Observable<any[]>;

  private readonly onDestroy$: Subject<void> = new Subject<void>();

  constructor(private _breadcrumbService: BreadcrumbService) {
  }

  ngOnInit(): void {
    this.breadcrumbs$ = this._breadcrumbService.activeBreadcrumbs$.pipe(takeUntil(this.onDestroy$), startWith([]));
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.unsubscribe();
  }
}
