namespace PlanningBook.Domain.Interfaces
{
    public interface IEntityBase<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
