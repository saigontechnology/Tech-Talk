using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Themes.Infrastructure.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningBook.Themes.Infrastructure.Entities
{
    // DEMO only
    public class Invoice : EntityBase<Guid>, IDateAudited
    {
        public PaymentStatus PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ActualyTotalAmout { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
