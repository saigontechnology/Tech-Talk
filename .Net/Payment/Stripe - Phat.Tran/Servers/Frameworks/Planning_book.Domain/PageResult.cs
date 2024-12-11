namespace PlanningBook.Domain
{
    public class PageResult<TResult>
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int NumberItemsPerPage { get; set; }
        public int TotalPage => (TotalItems / NumberItemsPerPage) + ((TotalItems % NumberItemsPerPage) > 0 ? 1 : 0);
        public IEnumerable<TResult> Data { get; set; }
    }
}
