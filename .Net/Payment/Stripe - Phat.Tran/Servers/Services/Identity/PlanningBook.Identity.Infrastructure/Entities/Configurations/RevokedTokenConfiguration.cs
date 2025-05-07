using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;

namespace PlanningBook.Identity.Infrastructure.Entities.Configurations
{
    public class RevokedTokenConfiguration : BaseRelationDbEntityTypeConfiguration<RevokedToken>
    {
        public override void Configure(EntityTypeBuilder<RevokedToken> builder)
        {
            base.Configure(builder);
        }
    }
}
