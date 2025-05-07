namespace PlanningBook.Domain.Interfaces
{
    public interface ISoftDeleted
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
