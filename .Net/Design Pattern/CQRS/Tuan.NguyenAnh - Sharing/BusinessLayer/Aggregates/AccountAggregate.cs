using BusinessLayer.Events;
using DataLayer.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Aggregates
{
    public class AccountAggregate : AggregateRoot
    {
        public override AggregateState CreateState() => new AccountAggregateState();

        public void OpenAccount(Guid owner, string code)
        {
            var e = new AccountOpened(owner, code);
            Apply(e);
        }

        public void DepositMoney(decimal amount, Guid transaction)
        {
            var e = new MoneyDeposited(amount, transaction);
            Apply(e);
        }

        public void WithdrawMoney(decimal amount, Guid transaction)
        {
            var e = new MoneyWithdrawn(amount, transaction);
            Apply(e);
        }

        public void CloseAccount(string reason)
        {
            var e = new AccountClosed(reason);
            Apply(e);
        }
    }
}
