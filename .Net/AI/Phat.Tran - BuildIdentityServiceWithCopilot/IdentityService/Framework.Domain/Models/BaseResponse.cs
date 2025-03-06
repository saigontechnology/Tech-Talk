namespace Framework.Domain.Models
{
    public class BaseResponse<TData>
    {
        public TData? Data { get; set; }
        public bool Success { get; set; }
        public Dictionary<int, string>? Messages { get; set; }

        public static BaseResponse<TData> SuccessResponse(TData data, Dictionary<int, string>? messages = null)
        {
            return new BaseResponse<TData>
            {
                Success = true,
                Data = data,
                Messages = messages
            };
        }

        public static BaseResponse<TData> FailureResponse(Dictionary<int, string>? messages = null)
        {
            return new BaseResponse<TData>
            {
                Success = false,
                Messages = messages
            };
        }
    }
}
