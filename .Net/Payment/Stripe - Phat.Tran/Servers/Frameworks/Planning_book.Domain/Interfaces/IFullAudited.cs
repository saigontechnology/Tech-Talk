namespace PlanningBook.Domain.Interfaces
{
    public interface IFullAudited<TPrimaryKey> : IDateAudited, IAuthorAudited<TPrimaryKey>
    {
    }
}
