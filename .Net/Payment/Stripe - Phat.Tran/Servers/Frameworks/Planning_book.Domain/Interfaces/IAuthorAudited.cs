namespace PlanningBook.Domain.Interfaces
{
    public interface IAuthorAudited<TPrimaryKey>
    {
        TPrimaryKey? CreatedBy { get; set; }
        TPrimaryKey? UpdatedBy { get; set; }
    }
}
