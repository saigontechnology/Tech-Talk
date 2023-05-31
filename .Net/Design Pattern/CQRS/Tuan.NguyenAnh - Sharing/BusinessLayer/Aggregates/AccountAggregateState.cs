using BusinessLayer.Events;
using DataLayer.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Aggregates
{
    public class AccountAggregateState : AggregateState
    {
        public Guid Owner { get; set; }
        public decimal CurrentBalance { get; set; }
        public string CurrentStatus { get; set; }
        public bool IsOverdrawn => CurrentBalance < 0;

        public void When(MoneyDeposited e)
        {
            CurrentBalance += e.Amount;
        }

        public void When(MoneyWithdrawn e)
        {
            CurrentBalance -= e.Amount;
        }

        public void When(AccountOpened e)
        {
            Owner = e.Owner;
            CurrentStatus = "Open";
        }

        public void When(AccountClosed e)
        {
            CurrentStatus = "Closed";
        }
    }
}
