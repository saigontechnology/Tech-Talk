using System.ServiceModel;
using ProtoBuf.Grpc;
using VinhNgo.gRPC.Shared.Models;

namespace VinhNgo.gRPC.Shared.Contracts;

public interface IToDoService
{
    [OperationContract]
    Task<TodoReply> SayHelloAsync(TodoModel request, CallContext context = default);
    
    [OperationContract]
    Task<TodoReply> SayHelloAsync1(TodoModel request, CallContext context = default);
}