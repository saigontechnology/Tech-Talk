using Grpc.Core;
using VinhNgo.Sample.gRPC.Entities;

namespace VinhNgo.Sample.gRPC.Services;

public class GreeterService(ILogger<GreeterService> _logger, MainDBContext _dbContext) : Greeter.GreeterBase
{
    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        // await Task.Delay(10000, context.CancellationToken);
        
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Name is required."));
        }
        
        return new HelloReply
        {
            Message = "Hello " + request.Name
        };
    }

    public override async Task<CountCreated> BulkCreate(IAsyncStreamReader<CreateRequest> requestStream, ServerCallContext context)
    {
        var count = 0;
        await foreach (var userRequest in requestStream.ReadAllAsync(context.CancellationToken)) {
            
            var user = await CreateUser(userRequest, context.CancellationToken);

            count++;
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new CountCreated
        {
            Count = count
        };
    }

    private async Task<UserEntity> CreateUser(CreateRequest request, CancellationToken cancellationToken = default)
    {
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };
        
        await _dbContext.AddAsync(user, cancellationToken);

        return user;
    }
}