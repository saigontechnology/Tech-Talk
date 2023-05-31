using BusinessLayer.Events;
using DataLayer.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Aggregates
{
    public class TransferAggregate : AggregateRoot
    {
        public override AggregateState CreateState() => new TransferAggregateState();

        public void StartTransfer(Guid fromAccount, Guid toAccount, decimal amount)
        {
            var e = new TransferStarted(fromAccount, toAccount, amount);
            Apply(e);
        }

        public void UpdateTransfer(string activity)
        {
            var e = new TransferUpdated(activity);
            Apply(e);
        }

        public void CompleteTransfer()
        {
            var e = new TransferCompleted();
            Apply(e);
        }
    }
}
