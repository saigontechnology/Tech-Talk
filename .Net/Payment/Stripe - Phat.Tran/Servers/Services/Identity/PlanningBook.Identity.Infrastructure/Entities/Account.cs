using Microsoft.AspNetCore.Identity;
using PlanningBook.Domain.Interfaces;

namespace PlanningBook.Identity.Infrastructure.Entities
{
    public class Account : IdentityUser<Guid>, IEntityBase<Guid>, IFullAudited<Guid?>,
        ISoftDeleted, IActiveEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        //public virtual Account? CreateByAccount { get; set; }
        public Guid? UpdatedBy { get; set; }
        //public virtual Account? UpdatedByAccount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; }

        // One to many
        public ICollection<AccountClaim> Claims { get; set; }
        public ICollection<AccountLogin> Logins { get; set; }
        public ICollection<AccountRole> Roles { get; set; }
        public ICollection<AccountToken> Tokens { get; set; }
        public ICollection<AccountPerson> Persons { get; set; }
    }
}
