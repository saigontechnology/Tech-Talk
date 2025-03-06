namespace Framework.Domain.Core.Interfaces
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
