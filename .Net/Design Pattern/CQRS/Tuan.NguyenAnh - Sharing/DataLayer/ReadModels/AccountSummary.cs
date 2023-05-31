using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Query
{
    public class AccountSummary
    {
        public Guid OwnerIdentifier { get; set; }

        public string AccountCode { get; set; }
        public Guid AccountIdentifier { get; set; }
        public string AccountStatus { get; set; }
        public Decimal AccountBalance { get; set; }
    }
}
