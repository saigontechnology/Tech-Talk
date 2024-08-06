using SharedDomains;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password", secret: true);
var productServer = builder.AddSqlServer(DomainConst.EndpointConst.DB_SQL_PRODUCT, password)
    .WithDataVolume();
var productDb = productServer.AddDatabase(DomainConst.DB_PRODUCT);

var product = builder.AddProject<Projects.WebApi_Second>(DomainConst.EndpointConst.API_PRODUCT)
    .WithReference(productDb);

var store = builder.AddProject<Projects.WebAPI>(DomainConst.EndpointConst.API_STORE);

product = product.WithReference(store);
store = store.WithReference(product);

builder.AddProject<Projects.WebMVC>(DomainConst.EndpointConst.WEB_MVC)
    .WithExternalHttpEndpoints()
    .WithReference(product)
    .WithReference(store);

builder.AddNpmApp("react", "../reactapp")
    .WithReference(store)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

if (OperatingSystem.IsWindows())
{
    builder.AddProject<Projects.Form2>(DomainConst.EndpointConst.APP_FORM)
        .WithReference(store)
        .ExcludeFromManifest();

    builder.AddProject<Projects.ConsoleApp>(DomainConst.EndpointConst.APP_CONSOLE)
        .WithReference(store);
}

builder.Build().Run();
