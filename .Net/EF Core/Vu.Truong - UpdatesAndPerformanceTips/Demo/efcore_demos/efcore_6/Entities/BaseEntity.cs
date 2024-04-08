namespace efcore_demos.Entities;
internal class BaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
}
