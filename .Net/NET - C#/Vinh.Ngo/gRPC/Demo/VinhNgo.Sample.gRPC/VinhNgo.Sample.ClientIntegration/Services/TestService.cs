using Grpc.Core;
using VinhNgo.gRPC.Shared.Contracts;
using VinhNgo.gRPC.Shared.Models;
using VinhNgo.Sample.gRPC;

namespace Sts.Sample.Grpc.ClientIntegration.Services;

public class TestService
{
    private readonly Greeter.GreeterClient _client;
    private readonly IToDoService _toDoService;
    private readonly ILogger<TestService> _logger;

    public TestService(Greeter.GreeterClient client, ILogger<TestService> logger,
        IToDoService toDoService)
    {
        _client = client;
        _logger = logger;
        _toDoService = toDoService;
    }

    public async Task<string> SayHelloAsync(string? name)
    {
        try
        {
            var headers = new Metadata();
            // headers.Add("X-API-KEY", "test-grpc-key");

            // var deadline = DateTime.UtcNow.AddSeconds(5);

            var reply = await _client.SayHelloAsync(new HelloRequest {Name = string.IsNullOrWhiteSpace(name) ? "" : name}, headers);

            return reply.Message;

            // var cts = new CancellationTokenSource();
            // var reply = _client.SayHelloAsync(new HelloRequest { Name = name }, headers, cancellationToken: cts.Token);
            //
            // // Cancel the call after 2 seconds
            // cts.CancelAfter(TimeSpan.FromSeconds(2));
            //
            // var response = await reply.ResponseAsync;
            // return response.Message;
        }
        catch (RpcException ex)
        {
            switch (ex.StatusCode)
            {
                case StatusCode.InvalidArgument:
                    _logger.LogError("Error: " + ex.Status.Detail);
                    break;
                case StatusCode.Internal:
                    _logger.LogError("Error: " + ex.Status.Detail);
                    break;
                case StatusCode.Cancelled:
                    _logger.LogWarning("Call was cancelled.");
                    break;
                case StatusCode.DeadlineExceeded:
                    _logger.LogWarning("Deadline timeout");
                    break;
            }

            _logger.LogError("Error: " + ex.Status.Detail);
        }

        return "";
    }
    
    public async Task ClientStream(string name)
    {
        var headers = new Metadata();
        headers.Add("X-API-KEY", "test-grpc-key");
        
        // var deadline = DateTime.UtcNow.AddSeconds(5);
        var cts = new CancellationTokenSource();
        // using var call = _client.BulkCreate(headers, deadline: deadline, cancellationToken: cts.Token);
        using var call = _client.BulkCreate(headers, cancellationToken: cts.Token);
        
        for (int i = 0; i < 5; i++) {
            await call.RequestStream.WriteAsync(new CreateRequest
            {
                FirstName = $"cancel {i}",
                LastName = "cancel",
                Password = "cancel123",
                PhoneNumber = "cancel1233",
                Email = "cancel"
            }, cancellationToken: cts.Token);

            if (i == 3)
            {
                await cts.CancelAsync();
            }
            
            await Task.Delay(1000, cts.Token);
        }
        
        await call.RequestStream.CompleteAsync();

        var response = await call;
        _logger.LogInformation(response.Count.ToString());
    }

    public async Task<string> TestCodeFirst(string name)
    {
        var reply = await _toDoService.SayHelloAsync(new TodoModel
        {
            Message = name
        });

        return reply.Name;
    }
}