using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;

namespace PlanningBook.Identity.Infrastructure.Entities.Configurations
{
    public class AccountLoginConfiguration : BaseRelationDbEntityTypeConfiguration<AccountLogin>
    {
        public override void Configure(EntityTypeBuilder<AccountLogin> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.UserId)
                .HasColumnName(nameof(AccountLogin.AccountId));
            builder.Ignore(p => p.AccountId);

            builder.HasOne(l => l.Account)
                .WithMany(a => a.Logins)
                .HasForeignKey(l => l.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
