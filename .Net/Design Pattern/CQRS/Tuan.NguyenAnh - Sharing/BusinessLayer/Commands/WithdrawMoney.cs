using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Command
{
    public class WithdrawMoney : CommandBase
    {
        public decimal Amount { get; set; }
        public Guid Transaction { get; set; }

        public WithdrawMoney(Guid account, decimal amount, Guid transaction)
        {
            AggregateIdentifier = account;
            Amount = amount;
            Transaction = transaction;
        }
    }
}
