
using BusinessLayer.Events;
using BusinessLayer.Serivces;
using DataLayer.Service.Query;

namespace BusinessLayer.EventHandlers
{
    public class AccountEventHandler
    {
        private readonly IQueryStore _store;

        public AccountEventHandler(IEventQueue queue, IQueryStore store)
        {
            queue.Subscribe<AccountOpened>(Handle);
            queue.Subscribe<AccountClosed>(Handle);
            queue.Subscribe<MoneyDeposited>(Handle);
            queue.Subscribe<MoneyWithdrawn>(Handle);

            _store = store;
        }

        public void Handle(AccountOpened c)
        {
            _store.InsertAccount(c.IdentityTenant, c.AggregateIdentifier, c.Code, "Open", c.Owner);
        }

        public void Handle(AccountClosed c)
        {
            _store.UpdateAccountStatus(c.AggregateIdentifier, "Closed");
        }

        public void Handle(MoneyDeposited c)
        {
            _store.IncreaseAccountBalance(c.AggregateIdentifier, c.Amount);
        }

        public void Handle(MoneyWithdrawn c)
        {
            _store.DecreaseAccountBalance(c.AggregateIdentifier, c.Amount);
        }
    }
}