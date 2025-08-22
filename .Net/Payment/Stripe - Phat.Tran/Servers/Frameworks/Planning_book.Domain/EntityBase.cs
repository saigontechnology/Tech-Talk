using PlanningBook.Domain.Interfaces;

namespace PlanningBook.Domain
{
    public abstract class EntityBase<TPrimaryKey> : IEntityBase<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}
