using System;

namespace BusinessLayer.Events
{
    public class MoneyDeposited : EventBase
    {
        public decimal Amount { get; set; }
        public Guid Transaction { get; set; }
        
        public MoneyDeposited(decimal amount, Guid transaction)
        {
            Amount = amount;
            Transaction = transaction;
        }
    }
}
