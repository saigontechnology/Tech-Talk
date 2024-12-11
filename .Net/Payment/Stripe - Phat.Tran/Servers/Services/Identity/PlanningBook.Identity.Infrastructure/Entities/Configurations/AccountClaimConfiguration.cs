using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;

namespace PlanningBook.Identity.Infrastructure.Entities.Configurations
{
    public class AccountClaimConfiguration : BaseRelationDbEntityTypeConfiguration<AccountClaim>
    {
        public override void Configure(EntityTypeBuilder<AccountClaim> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.UserId)
                .HasColumnName(nameof(AccountClaim.AccountId));
            builder.Ignore(p => p.AccountId);

            builder.HasOne(ac => ac.Account)
                .WithMany(a => a.Claims)
                .HasForeignKey(ac => ac.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
