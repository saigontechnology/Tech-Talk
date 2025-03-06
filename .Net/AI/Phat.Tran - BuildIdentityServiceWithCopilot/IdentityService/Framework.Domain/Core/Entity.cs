using Framework.Domain.Core.Interfaces;

namespace Framework.Domain.Core
{
    public class Entity<T> : IEntity<T>
    {
        public T Id { get; set ; }
    }
}
