namespace UnderstandingDependencies.Api.Models;

public class User
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = default!;
}
