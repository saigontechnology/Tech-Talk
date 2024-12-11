using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;
using PlanningBook.Identity.Infrastructure.Enums;

namespace PlanningBook.Identity.Infrastructure.Entities.Configurations
{
    public class RoleConfiguration : BaseRelationDbEntityTypeConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            #region Seed Data
            builder.HasData(
                new Role()
                {
                    Id = new Guid("4155f55f-b7e7-4529-865b-63f2ea7865fa"),
                    RoleType = RoleType.SysAdmin,
                    AppliedEntity = RoleAppliedEntity.Account,
                    Name = $"a_{RoleType.SysAdmin.ToString()}",
                    NormalizedName = $"a_{RoleType.SysAdmin.ToString().ToLower()}",
                },
                new Role()
                {
                    Id = new Guid("f8183db5-a09e-41d8-939b-c188a6247651"),
                    RoleType = RoleType.User,
                    AppliedEntity = RoleAppliedEntity.Account,
                    Name = $"a_{RoleType.User.ToString()}",
                    NormalizedName = $"a_{RoleType.User.ToString().ToLower()}"
                },
                new Role()
                {
                    Id = new Guid("a5a8bd75-c04b-4ad8-b1b7-b1db912ae8ef"),
                    RoleType = RoleType.Staff,
                    AppliedEntity = RoleAppliedEntity.Person,
                    Name = $"p_{RoleType.Staff.ToString()}",
                    NormalizedName = $"p_{RoleType.Staff.ToString().ToLower()}"
                },
                new Role()
                {
                    Id = new Guid("281c5aa9-f0bd-42bc-9c9f-060895fb4187"),
                    RoleType = RoleType.EndUser,
                    AppliedEntity = RoleAppliedEntity.Person,
                    Name = $"p_{RoleType.EndUser.ToString()}",
                    NormalizedName = $"p_{RoleType.EndUser.ToString().ToLower()}"
                });
            #endregion Seed Data
        }
    }
}
