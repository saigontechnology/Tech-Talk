using Grpc.Core;
using Grpc.Core.Interceptors;

namespace gRPC.WebApp.Interceptors;

public class ClientLoggingInterceptor : Interceptor
{
    private readonly ILogger<ClientLoggingInterceptor> _logger;

    public ClientLoggingInterceptor(ILogger<ClientLoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request, 
        ClientInterceptorContext<TRequest, TResponse> context, 
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("====Starting call. Type/Method: {Type} / {Method}", context.Method.Type, context.Method.Name);
        return continuation(request, context);
    }
}