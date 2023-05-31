using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.RequestDto
{
    public class NewTransactionDto
    {
        public Guid FromAccount { get; set; }
        public Guid ToAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
