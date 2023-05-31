using BusinessLayer.Aggregates;
using BusinessLayer.Command;
using BusinessLayer.Helper;
using BusinessLayer.Serivces;
using BusinessLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.CommandHandlers
{
    public class TransferCommandHandler
    {
        private readonly IEventRepository _repository;
        private readonly IEventQueue _publisher;

        public TransferCommandHandler(ICommandQueue commander, IEventQueue publisher, IEventRepository eventRepository)
        {
            _repository = eventRepository;
            _publisher = publisher;

            commander.Subscribe<StartTransfer>(Handle);
            commander.Subscribe<UpdateTransfer>(Handle);
            commander.Subscribe<CompleteTransfer>(Handle);

        }

        private void Commit(TransferAggregate aggregate)
        {
            var changes = _repository.Save(aggregate);
            foreach (var change in changes)
                _publisher.Publish(change);
        }

        public void Handle(StartTransfer c)
        {
            var accountFrom = _repository.Get<AccountAggregate>(c.FromAccount);
            if(((AccountAggregateState)accountFrom.State).CurrentBalance < c.Amount)
            {
                throw new Exception("Not enough money to transfer");
            }

            var aggregate = new TransferAggregate { AggregateIdentifier = c.AggregateIdentifier };
            aggregate.StartTransfer(c.FromAccount, c.ToAccount, c.Amount);
            Commit(aggregate);
        }

        public void Handle(UpdateTransfer c)
        {
            var aggregate = _repository.Get<TransferAggregate>(c.AggregateIdentifier);
            aggregate.UpdateTransfer(c.Activity);
            Commit(aggregate);
        }

        public void Handle(CompleteTransfer c)
        {
            var aggregate = _repository.Get<TransferAggregate>(c.AggregateIdentifier);
            aggregate.CompleteTransfer();
            Commit(aggregate);
        }
    }
}
