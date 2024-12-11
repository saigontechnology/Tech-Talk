using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningBook.DBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningBook.Themes.Infrastructure.Entities.Configurations
{
    public class StripeCustomerConfiguration : BaseRelationDbEntityTypeConfiguration<StripeCustomer>
    {
        public override void Configure(EntityTypeBuilder<StripeCustomer> builder)
        {
            base.Configure(builder);
        }
    }
}
