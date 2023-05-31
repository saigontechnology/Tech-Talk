using BusinessLayer.Helper;
using BusinessLayer.Serivces;
using DataLayer;
using DataLayer.Exceptions;
using DataLayer.Service.Command;
using DataLayer.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class SnapshotRepository : ISnapshotRepository
    {
        private readonly ISnapshotStore _snapshotStore;
        private readonly IEventRepository _eventRepository;
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Constructs a new SnapshotRepository instance.
        /// </summary>
        /// <param name="eventStore">Store where events are persisted</param>
        /// <param name="eventRepository">Repository to get aggregates from the event store</param>
        /// <param name="snapshotStore">Store where snapshots are persisted</param>
        /// <param name="snapshotStrategy">Strategy used to determine when to take a snapshot</param>
        public SnapshotRepository(IEventStore eventStore, IEventRepository eventRepository, ISnapshotStore snapshotStore)
        {
            _eventStore = eventStore;
            _eventRepository = eventRepository;
            _snapshotStore = snapshotStore;
        }

        /// <summary>
        /// Saves the aggregate. Takes a snapshot if needed.
        /// </summary>
        public IEvent[] Save<T>(T aggregate, int? version = null) where T : AggregateRoot
        {
            // Take a snapshot if needed.
            TakeSnapshot(aggregate, false);

            // Return the stream of saved events to the caller so they can be published.
            return _eventRepository.Save(aggregate, version);
        }

        /// <summary>
        /// Gets list of aggregates.
        /// </summary>
        public List<T> GetList<T>() where T : AggregateRoot
        {
            var listAggregates = new List<T>();
            var listSnapshots = _snapshotStore.GetList();

            // If there is no snapshot return empty list
            if (listSnapshots.Count == 0)
                return listAggregates;

            foreach (var snapshot in listSnapshots)
            {
                var aggregate = AggregateFactory<T>.CreateAggregate();

                aggregate.AggregateIdentifier = snapshot.AggregateIdentifier;
                aggregate.AggregateVersion = snapshot.AggregateVersion;
                aggregate.State = _eventStore.Serializer.Deserialize<AggregateState>(snapshot.AggregateState, aggregate.CreateState().GetType());

                listAggregates.Add(aggregate);
            }

            return listAggregates;
        }

        /// <summary>
        /// Gets the aggregate.
        /// </summary>
        public T Get<T>(Guid aggregateId) where T : AggregateRoot
        {
            // If it is not in the cache then load the aggregate from the most recent snapshot.
            var aggregate = AggregateFactory<T>.CreateAggregate();
            var snapshotVersion = RestoreAggregateFromSnapshot(aggregateId, aggregate);

            // If there is no snapshot then load the aggregate directly from the event store.
            if (snapshotVersion == -1)
                return _eventRepository.Get<T>(aggregateId);

            return aggregate;
        }

        /// <summary>
        /// Loads the aggregate from the most recent snapshot.
        /// </summary>
        /// <returns>
        /// Returns the version number for the aggregate when the snapshot was taken.
        /// </returns>
        private int RestoreAggregateFromSnapshot<T>(Guid id, T aggregate) where T : AggregateRoot
        {
            var snapshot = _snapshotStore.Get(id);

            if (snapshot == null)
                return -1;

            aggregate.AggregateIdentifier = snapshot.AggregateIdentifier;
            aggregate.AggregateVersion = snapshot.AggregateVersion;
            aggregate.State = _eventStore.Serializer.Deserialize<AggregateState>(snapshot.AggregateState, aggregate.CreateState().GetType());

            return snapshot.AggregateVersion;
        }

        /// <summary>
        /// Saves a snapshot of the aggregate if the strategy indicates a snapshot should now be taken.
        /// </summary>
        private void TakeSnapshot(AggregateRoot aggregate, bool force)
        {
            var snapshot = new Snapshot
            {
                AggregateIdentifier = aggregate.AggregateIdentifier,
                AggregateVersion = aggregate.AggregateVersion,
                AggregateState = _eventStore.Serializer.Serialize<AggregateState>(aggregate.State)
            };

            snapshot.AggregateVersion = aggregate.AggregateVersion + aggregate.GetUncommittedChanges().Length;

            _snapshotStore.Save(snapshot);
        }

       
    }
}
