using ProtoBuf.Grpc;
using VinhNgo.gRPC.Shared.Contracts;
using VinhNgo.gRPC.Shared.Models;

namespace VinhNgo.Sample.gRPC.Services;

public class ToDoService : IToDoService
{
    public Task<TodoReply> SayHelloAsync(TodoModel request, CallContext context = default)
    {
        return Task.FromResult(
            new TodoReply
            {
                Name = $"Hello {request.Message}"
            });
    }

    public Task<TodoReply> SayHelloAsync1(TodoModel request, CallContext context = default)
    {
        return Task.FromResult(
            new TodoReply
            {
                Name = $"Hello {request.Message}"
            });
    }
}