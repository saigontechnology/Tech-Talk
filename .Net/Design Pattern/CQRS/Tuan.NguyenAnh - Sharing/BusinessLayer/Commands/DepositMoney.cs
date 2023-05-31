using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Command
{
    public class DepositMoney : CommandBase
    {
        public decimal Amount { get; set; }
        public Guid Transaction { get; set; }

        public DepositMoney(Guid account, decimal amount, Guid transaction)
        {
            AggregateIdentifier = account;
            Amount = amount;
            Transaction = transaction;
        }

        public DepositMoney(Guid account, decimal amount)
            : this(account, amount, Guid.Empty)
        {

        }
    }
}
