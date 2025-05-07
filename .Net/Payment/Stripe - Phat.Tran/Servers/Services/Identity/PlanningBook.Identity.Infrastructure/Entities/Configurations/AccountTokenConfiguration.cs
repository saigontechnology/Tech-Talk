using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;

namespace PlanningBook.Identity.Infrastructure.Entities.Configurations
{
    public class AccountTokenConfiguration : BaseRelationDbEntityTypeConfiguration<AccountToken>
    {
        public override void Configure(EntityTypeBuilder<AccountToken> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.UserId)
                .HasColumnName(nameof(AccountToken.AccountId));
            builder.Ignore(p => p.AccountId);

            builder.Property(p => p.Value)
                .HasColumnName(nameof(AccountToken.Token));
            builder.Ignore(p => p.Token);

            builder.HasOne(t => t.Account)
                .WithMany(a => a.Tokens)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
