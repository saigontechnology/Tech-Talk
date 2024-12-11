using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;

namespace PlanningBook.Identity.Infrastructure.Entities
{
    public class RevokedToken : EntityBase<string>, IDateAudited, IAuthorAudited<Guid?>
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
