using PlanningBook.Domain.Interfaces;

namespace PlanningBook.Identity.Infrastructure.Entities
{
    public class AccountPerson : IDateAudited
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public Guid PersonId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
