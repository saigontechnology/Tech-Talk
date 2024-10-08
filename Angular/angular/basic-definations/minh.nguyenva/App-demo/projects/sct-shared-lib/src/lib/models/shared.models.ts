
export interface Page<T> {
  content: T[];
  empty: boolean;
  first: boolean;
  last: boolean;
  number: number;
  numberOfElements: number;
  pageable: Pageable;
  size: number;
  sort: Sort;
  totalElements: number;
  totalPages: number;
}

export interface Pageable {
  offset: number;
  pageNumber: number;
  pageSize: number;
  paged: boolean;
  sort: Sort;
  unpaged: boolean;
}

export interface Sort {
  unsorted: boolean;
  sorted: boolean;
  empty: boolean;
}

export interface Pagination {
  start: number;
  itemsPerPage: number;
  totalElements?: number;
}

export interface PageableRequest {
  term?: string;
  page: number;
  size: number;
  sort?: string[];
}