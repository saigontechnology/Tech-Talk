import { NgClass, NgStyle } from '@angular/common';
import { Observable } from 'rxjs';

export enum ColumnDataType {
  STRING = 0,
  OBSERVABLE = 1,
}

export interface Column {
  name: string;
  displayName: string | Observable<any>;
  displayType: ColumnDataType;
  displayFormat?: string[];
  sort?: SortType;
  ngClass?: NgClass['ngClass'];
  ngStyle?: NgStyle['ngStyle'];
}

export interface Column {
  name: string;
  displayName: string | Observable<any>;
  displayType: ColumnDataType;
  displayFormat?: string[];
  sort?: SortType;
  ngClass?: NgClass['ngClass'];
  ngStyle?: NgStyle['ngStyle'];
}

export interface ColumnData {
  data: any;
  type: ColumnDataType;
  pipeFormat?: string[];
  ngClass?: NgClass['ngClass'];
  ngStyle?: NgStyle['ngStyle'];
  iconClass?: NgClass['ngClass'];
  iconStyle?: NgStyle['ngStyle'];
  sort?: SortType;
}

export interface tableDataSource {
  rowData: any;
  [key: string]: ColumnData;
}
//sort

export enum SortType {
  ASC = 'asc',
  DESC = 'desc',
  EMPTY = 'empty',
}

export const SORT_TYPE = {
  ASC: 'asc',
  DESC: 'desc',
  EMPTY: 'empty',
};

export const compare = (v1: string | number, v2: string | number) => (v1 < v2 ? -1 : v1 > v2 ? 1 : 0);

// pagination

export enum PaginationOptions {
  totalElements = 0,
  page = 1,
  pageSize = 4,
}
