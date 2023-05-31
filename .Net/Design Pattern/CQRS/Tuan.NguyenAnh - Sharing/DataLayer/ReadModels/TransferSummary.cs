using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Query
{
    public class TransferSummary
    {
        public Guid TransferIdentifier { get; set; }
        public String TransferStatus { get; set; }
        public Decimal TransferAmount { get; set; }

        public Guid FromAccountIdentifier { get; set; }

        public Guid ToAccountIdentifier { get; set; }
    }
}
