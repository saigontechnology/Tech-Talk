using System;


namespace BusinessLayer.Events
{
    public class MoneyWithdrawn : EventBase
    {
        public decimal Amount { get; set; }
        public Guid Transaction { get; set; }
        
        public MoneyWithdrawn(decimal amount, Guid transaction)
        {
            Amount = amount;
            Transaction = transaction;
        }
    }
}
