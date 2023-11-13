import {
  AfterContentInit,
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  Directive,
  EventEmitter,
  Input,
  OnInit,
  Output,
  QueryList,
  TemplateRef,
  ViewChildren,
} from '@angular/core';
import { SortEvent, NgbdSortableHeader } from './../../directives/sortable.directive';
import { Column, ColumnDataType, SORT_TYPE, tableDataSource } from './table.model';

@Directive({
  selector: '[sct-table-template]',
  host: {},
})
export class SctTableTemplate {
  @Input('sct-table-template') name: string = '';

  constructor(public template: TemplateRef<any>) {}

  getType(): string {
    return this.name;
  }
}

@Component({
  selector: 'sct-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TableComponent implements OnInit, AfterViewInit, AfterContentInit {
  @ContentChildren(SctTableTemplate) templates: QueryList<any> | any;

  @Input() tableName: string = '';
  @Input() columns: Column[] = [];
  @Input() dataSource: tableDataSource[] = [];

  @Output() sortChange: EventEmitter<string[]> = new EventEmitter<string[]>();

  @ViewChildren(NgbdSortableHeader) headers!: QueryList<NgbdSortableHeader>;

  readonly ColumnDataType = ColumnDataType;

  SORT_TYPE = SORT_TYPE;
  columnsTemplates: { [key: string]: TemplateRef<HTMLElement> } = {};
  rowTemplate: TemplateRef<HTMLElement> | any;
  headerTemplate: TemplateRef<HTMLElement> | any;

  constructor() {}

  ngOnInit(): void {}

  ngAfterContentInit(): void {
    const columnName = this.columns.map((e) => e.name);
    this.templates.forEach((item: any) => {
      const type = item.getType();
      switch (type) {
        case 'headerTemplate':
          this.headerTemplate = item.template;
          break;

        case 'rowTemplate':
          this.rowTemplate = item.template;
          break;

        default:
          if (columnName.includes(type)) {
            this.columnsTemplates[type] = item.template;
          }
      }
    });
  }
  ngAfterViewInit(): void {}
  onSort({ column, direction }: SortEvent) {
    // resetting other headers
    this.headers.forEach((header) => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });
    this._emitSort(column, direction);
  }
  private _emitSort(column: string | number, direction: string) {
    const params = direction === '' ? '' : `${column.toString()},${direction}`;
    const sortParams = [params];
    this.sortChange.emit(sortParams);
  }
}
