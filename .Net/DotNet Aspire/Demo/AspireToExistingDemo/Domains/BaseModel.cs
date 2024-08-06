namespace SharedDomains;
public class BaseModel<T>
{
    public T Id { get; set; }
}

public class BaseModel : BaseModel<Guid>
{
    public BaseModel()
    {
        Id = Guid.NewGuid();
    }

    public virtual void Generate()
    {

    }
}

public class IdNameModel<T>(T id, string name = null)
{
    public T Id { get; set; } = id;
    public string Name { get; set; } = name;
}
