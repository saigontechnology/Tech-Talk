using SharedDomains;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis(DomainConst.EndpointConst.SERVICE_REDIS);

var product = builder.AddProject<Projects.ProductApi>(DomainConst.EndpointConst.API_PRODUCT)
    .WithReference(redis)
    .WithReplicas(2);

var frontend = builder.AddNpmApp(DomainConst.EndpointConst.API_ORDER, "../order-api", "watch")
    .WithReference(product)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddHealthChecksUI("healthchecksui")
    .WithReference(redis)
    .WithReference(product)
    .WithReference(frontend)
    .WithExternalHttpEndpoints();

builder.Build().Run();
