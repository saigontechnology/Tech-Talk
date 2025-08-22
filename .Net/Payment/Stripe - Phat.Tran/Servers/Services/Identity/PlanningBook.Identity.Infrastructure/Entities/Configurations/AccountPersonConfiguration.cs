using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;

namespace PlanningBook.Identity.Infrastructure.Entities.Configurations
{
    public class AccountPersonConfiguration : BaseRelationDbEntityTypeConfiguration<AccountPerson>
    {
        public override void Configure(EntityTypeBuilder<AccountPerson> builder)
        {
            base.Configure(builder);

            builder.HasKey(ap => new { ap.AccountId, ap.PersonId });

            builder.HasOne(ap => ap.Account)
                .WithMany(a => a.Persons)
                .HasForeignKey(ap => ap.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
