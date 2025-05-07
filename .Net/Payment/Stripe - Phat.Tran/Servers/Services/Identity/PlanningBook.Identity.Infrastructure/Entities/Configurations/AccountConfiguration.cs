using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;

namespace PlanningBook.Identity.Infrastructure.Entities.Configurations
{
    public class AccountConfiguration : BaseRelationDbEntityTypeConfiguration<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            //builder.HasMany(a => a.Claims)
            //    .WithOne(ac => ac.Account)
            //    .HasForeignKey(ac => ac.AccountId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(a => a.Logins)
            //    .WithOne(l => l.Account)
            //    .HasForeignKey(l => l.AccountId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(a => a.Roles)
            //    .WithOne(r => r.Account)
            //    .HasForeignKey(r => r.AccountId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(a => a.Tokens)
            //    .WithOne(t => t.Account)
            //    .HasForeignKey(t => t.AccountId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(a => a.Persons)
            //    .WithOne(p => p.Account)
            //    .HasForeignKey(p => p.AccountId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
