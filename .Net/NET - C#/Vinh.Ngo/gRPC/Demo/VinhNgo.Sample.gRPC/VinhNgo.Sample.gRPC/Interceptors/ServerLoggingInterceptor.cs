using Grpc.Core;
using Grpc.Core.Interceptors;

namespace VinhNgo.Sample.gRPC.Interceptors;

public class ServerLoggingInterceptor : Interceptor
{
    private readonly ILogger<ServerLoggingInterceptor> _logger;

    public ServerLoggingInterceptor(ILogger<ServerLoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, 
        ServerCallContext context, 
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Handling call. Method: {Method}", context.Method);
        var response = await continuation(request, context);
        _logger.LogInformation("Call handled. Method: {Method}", context.Method);
        return response;
    }
}