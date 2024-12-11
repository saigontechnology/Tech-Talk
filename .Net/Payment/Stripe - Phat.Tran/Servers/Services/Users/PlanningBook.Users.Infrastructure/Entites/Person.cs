using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;

namespace PlanningBook.Users.Infrastructure.Entites
{
    public class Person : EntityBase<Guid>, 
        IDateAudited, 
        IAuthorAudited<Guid?>, 
        IActiveEntity, 
        ISoftDeleted
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
