namespace efcore_demos.Entities;

internal interface IBaseEntity<T>
{
    T Id { get; set; }

    DateTimeOffset CreatedDate { get; set; }
    DateTimeOffset? UpdatedDate { get; set; }
}

internal interface IBaseEntity : IBaseEntity<Guid>
{
}

internal class BaseEntity<T> : IBaseEntity<T>
{
    public BaseEntity()
    {
        CreatedDate = DateTime.UtcNow;
    }

    public T Id { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}

internal class BaseEntity : BaseEntity<Guid>, IBaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}