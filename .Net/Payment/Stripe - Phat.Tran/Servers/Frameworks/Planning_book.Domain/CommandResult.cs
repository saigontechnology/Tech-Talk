namespace PlanningBook.Domain
{
    public class CommandResult<TData> : BaseResult<TData>
    {
        public static CommandResult<TData> Success(TData? data)
        {
            return new CommandResult<TData>()
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static CommandResult<TData> Failure(string? errorCode = null, string? errorMessages = null)
        {
            return new CommandResult<TData>()
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessages
            };
        }
    }
}
