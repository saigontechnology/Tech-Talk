namespace PlanningBook.Domain
{
    public class QueryResult<TData> : BaseResult<TData>
    {
        public static QueryResult<TData> Success(TData? data)
        {
            return new QueryResult<TData>()
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static QueryResult<TData> Failure(string? errorCode = null, string? errorMessages = null)
        {
            return new QueryResult<TData>()
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessages
            };
        }
    }
}
