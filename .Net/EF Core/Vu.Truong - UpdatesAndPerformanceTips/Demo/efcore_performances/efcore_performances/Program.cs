var container = Container.GetInstance();
var runner = Runner.GetInstance();

container.Build((services, configuration) =>
{
    container.AddDbContext(services, configuration);

    runner.Build(services, configuration);
});

await runner.ExecuteAsync();