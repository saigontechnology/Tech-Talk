
using BusinessLayer.Events;
using BusinessLayer.Serivces;
using DataLayer.Service.Query;


namespace BusinessLayer.EventHandlers
{
    public class UserEventHandler
    {
        private readonly IQueryStore _store;

        public UserEventHandler(IEventQueue queue, IQueryStore store)
        {
            _store = store;

            queue.Subscribe<UserRegistered>(Handle);
            queue.Subscribe<UserRenamed>(Handle);
        }

        public void Handle(UserRegistered c)
        {
            _store.InsertUser(c.IdentityTenant, c.AggregateIdentifier, c.FirstName + " " + c.LastName, c.Registered, c.Name, c.Password, c.Status);
        }

        public void Handle(UserRenamed c)
        {
            _store.UpdateName(c.AggregateIdentifier, c.FirstName + " " + c.LastName);
        }
    }
}
