namespace Core.Contracts;
public interface IBaseGuidModel
{
    Guid Id { get; set; }
}

public interface IBaseModel<T>
{
    T Id { get; set; }
}