namespace PlanningBook.Domain
{
    public class BaseResult<TData>
    {
        public TData? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
