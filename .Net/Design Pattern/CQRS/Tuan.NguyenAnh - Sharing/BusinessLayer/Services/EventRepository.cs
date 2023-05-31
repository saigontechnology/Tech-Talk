using BusinessLayer.Helper;
using DataLayer;
using DataLayer.Exceptions;
using DataLayer.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Serivces
{
    public class EventRepository : IEventRepository
    {
        private readonly IEventStore _store;

        public EventRepository(IEventStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        /// <summary>
        /// Gets an aggregate from the event store.
        /// </summary>
        public T Get<T>(Guid aggregate) where T : AggregateRoot
        {
            return Rehydrate<T>(aggregate);
        }

        /// <summary>
        /// Saves all uncommitted changes to the event store.
        /// </summary>
        public IEvent[] Save<T>(T aggregate, int? version) where T : AggregateRoot
        {
            // Get the list of events that are not yet saved. 
            var events = aggregate.FlushUncommittedChanges();

            // Save the uncommitted changes.
            _store.Save(aggregate, events);

            // return events to the caller to publish
            return events;
        }

        /// <summary>
        /// Loads an aggregate instance from the full history of events for that aggreate.
        /// </summary>
        private T Rehydrate<T>(Guid id) where T : AggregateRoot
        {
            // Get all the events for the aggregate.
            var events = _store.Get(id, -1, typeof(T).Name.Replace("Aggregate", string.Empty));

            if (!events.Any())
                throw new IdentityNotFoundException("Invalid aggregate's identifier");

            // Create and load the aggregate.
            var aggregate = AggregateFactory<T>.CreateAggregate();
            aggregate.Rehydrate(events);
            return aggregate;
        }
    }
}
