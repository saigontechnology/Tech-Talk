import {
  ChangeDetectionStrategy,
  Component,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { PAGINATION, PaginationEvent, ITEM_PER_PAGE } from './pagination.model';
@Component({
  selector: 'sct-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaginationComponent {
  @Input() page: number = PAGINATION.page;
  @Input() pageSize: number = PAGINATION.pageSize;
  @Input() totalElements: number = PAGINATION.totalElements;

  @Output() paginationChange = new EventEmitter<PaginationEvent>();

  itemPerPage = ITEM_PER_PAGE;

  onPageChange(page: number) {
    this.paginationChange.emit({ page, pageSize: this.pageSize });
  }

  onPageSizeChange(pageSize: number) {
    this.paginationChange.emit({ page: this.page, pageSize });
  }
}
