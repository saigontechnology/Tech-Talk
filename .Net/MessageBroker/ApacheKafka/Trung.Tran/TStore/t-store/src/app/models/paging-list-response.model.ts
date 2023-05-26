export interface PagingListResponse<T> {
    total: number;
    items: T[];
}