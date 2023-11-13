export const PAGINATION = {
  totalElements: 0,
  page: 1,
  pageSize: 5,
};

export const ITEM_PER_PAGE = [
  {
    value: 5 , displayName:'5 items per page'
},
  {
    value: 10 , displayName:'10 items per page'
  },
  {
    value: 20 , displayName:'20 items per page'
  },
];

export interface PaginationEvent {
  page: number;
  pageSize: number;
}
