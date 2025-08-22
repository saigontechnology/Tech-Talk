using PlanningBook.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningBook.Themes.Infrastructure.Entities
{
    public class InvoiceDetail : EntityBase<Guid>
    {
        public Guid InvoiceId { get; set; }
        public Guid ProductId { get; set; }
    }
}
