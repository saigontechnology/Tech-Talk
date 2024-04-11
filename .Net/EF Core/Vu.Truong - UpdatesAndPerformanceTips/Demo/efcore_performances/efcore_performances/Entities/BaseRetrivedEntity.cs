using System.ComponentModel.DataAnnotations.Schema;

namespace efcore_performances.Entities;

public interface IHasRetrieved
{
    DateTimeOffset? Retrieved { get; set; }
}

internal class BaseRetrievedEntity : BaseEntity, IHasRetrieved
{
    public BaseRetrievedEntity()
    {
        InstanceCreated = DateTime.UtcNow;
    }

    [NotMapped]
    public DateTimeOffset? Retrieved { get; set; }

    [NotMapped]
    public DateTimeOffset? InstanceCreated { get; set; }
}
