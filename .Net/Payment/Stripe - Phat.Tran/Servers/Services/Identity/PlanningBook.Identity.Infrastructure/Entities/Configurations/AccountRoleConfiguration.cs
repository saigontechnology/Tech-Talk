using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;

namespace PlanningBook.Identity.Infrastructure.Entities.Configurations
{
    public class AccountRoleConfiguration : BaseRelationDbEntityTypeConfiguration<AccountRole>
    {
        public override void Configure(EntityTypeBuilder<AccountRole> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.UserId)
                .HasColumnName(nameof(AccountRole.AccountId));
            builder.Ignore(p => p.AccountId);

            builder.HasOne(r => r.Account)
                .WithMany(a => a.Roles)
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
