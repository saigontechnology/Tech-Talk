using BusinessLayer.Aggregates;
using BusinessLayer.Command;
using BusinessLayer.Events;
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
	public class TransferProcessHandler
	{
		private readonly ICommandQueue _commander;
		private readonly IEventRepository _repository;

		public TransferProcessHandler(ICommandQueue commander, IEventQueue publisher, IEventRepository eventRepository)
		{
			_commander = commander;
			_repository = eventRepository;

			publisher.Subscribe<TransferStarted>(Handle);
			publisher.Subscribe<MoneyDeposited>(Handle);
			publisher.Subscribe<MoneyWithdrawn>(Handle);
		}

		public void Handle(TransferStarted e)
		{
			var withdrawal = new WithdrawMoney(e.FromAccount, e.Amount, e.AggregateIdentifier);
			_commander.Send(withdrawal);
		}

		public void Handle(MoneyWithdrawn e)
		{
			if (e.Transaction == Guid.Empty)
				return;

			var status = new UpdateTransfer(e.Transaction, "Debit Succeeded");
			_commander.Send(status);

			var transfer = (TransferAggregateState)_repository.Get<TransferAggregate>(e.Transaction).State;

			var deposit = new DepositMoney(transfer.ToAccount, e.Amount, e.Transaction);
			_commander.Send(deposit);
		}

		public void Handle(MoneyDeposited e)
		{
			if (e.Transaction == Guid.Empty)
				return;

			var status = new UpdateTransfer(e.Transaction, "Credit Succeeded");
			_commander.Send(status);

			var complete = new CompleteTransfer(e.Transaction);
			_commander.Send(complete);
		}
	}
}
