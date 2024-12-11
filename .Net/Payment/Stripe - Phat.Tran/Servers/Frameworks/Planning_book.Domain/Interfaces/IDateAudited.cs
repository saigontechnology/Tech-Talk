namespace PlanningBook.Domain.Interfaces
{
    public interface IDateAudited
    {
        DateTime? CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
    }
}
