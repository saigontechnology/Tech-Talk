namespace Core.Models;

public class BaseModel : BaseModel<Guid>, IBaseGuidModel
{
    public BaseModel()
    {
        Id = Guid.NewGuid();
    }
}

public class BaseModel<T> : IBaseModel<T>
{
    public T Id { get; set; }
}
