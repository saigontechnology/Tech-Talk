using BusinessLayer.Events;
using DataLayer.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Aggregates
{
    internal class TransferAggregateState : AggregateState
    {
        public Guid FromAccount { get; set; }
        public Guid ToAccount { get; set; }
        public decimal Amount { get; set; }
        public TransferStatus Status { get; set; }

        public void When(TransferStarted e)
        {
            FromAccount = e.FromAccount;
            ToAccount = e.ToAccount;
            Amount = e.Amount;
            Status = TransferStatus.Started;
        }

        public void When(TransferUpdated e)
        {
            Status = TransferStatus.Updated;
        }

        public void When(TransferCompleted e)
        {
            Status = TransferStatus.Completed;
        }
    }
}
