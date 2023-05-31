using DataLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Command
{
    public abstract class AggregateRoot
    {
        /// <summary>
        /// Changes to the state of the aggregate that are not yet committed to a persistent event store.
        /// </summary>
        private readonly List<IEvent> _changes = new List<IEvent>();

        /// <summary>
        /// Represents the state for the aggregate.
        /// </summary>
        public AggregateState State { get; set; }

        /// <summary>
        /// Uniquely identifies the aggregate.
        /// </summary>
        public Guid AggregateIdentifier { get; set; }

        /// <summary>
        /// Current version of the aggregate.
        /// </summary>
        public int AggregateVersion { get; set; }

        /// <summary>
        /// Every aggregate must override this method to create the object that holds its current state.
        /// </summary>
        public abstract AggregateState CreateState();

        /// <summary>
        /// Returns all uncommitted changes. 
        /// </summary>
        /// <returns></returns>
        public IEvent[] GetUncommittedChanges()
        {
            lock (_changes)
            {
                return _changes.ToArray();
            }
        }

        /// <summary>
        /// Returns all uncommitted changes and clears them from the aggregate.
        /// </summary>
        public IEvent[] FlushUncommittedChanges()
        {
            lock (_changes)
            {
                var changes = _changes.ToArray();

                var i = 0;

                foreach (var change in changes)
                {
                    if (change.AggregateIdentifier == Guid.Empty && AggregateIdentifier == Guid.Empty)
                        throw new IdentityNotFoundException("Invalid Aggregate's identifier");

                    if (change.AggregateIdentifier == Guid.Empty)
                        change.AggregateIdentifier = AggregateIdentifier;

                    i++;

                    change.AggregateVersion = AggregateVersion + i;
                    change.EventTime = DateTimeOffset.UtcNow;
                }

                AggregateVersion = AggregateVersion + changes.Length;

                _changes.Clear();

                return changes;
            }
        }

        /// <summary>
        /// Loads an aggregate from a stream of events.
        /// </summary>
        public void Rehydrate(IEnumerable<IEvent> history)
        {
            lock (_changes)
            {
                foreach (var change in history.ToArray())
                {
                    if (change.AggregateVersion != AggregateVersion + 1)
                        throw new Exception("Invalid event order");

                    ApplyEvent(change);

                    AggregateIdentifier = change.AggregateIdentifier;
                    AggregateVersion++;
                }
            }
        }

        /// <summary>
        /// Applies a change to the aggregate state AND appends the event to the history of uncommited changes.
        /// </summary>
        protected void Apply(IEvent change)
        {
            lock (_changes)
            {
                ApplyEvent(change);
                _changes.Add(change);
            }
        }

        /// <summary>
        /// Applies a change to the aggregate state. This method is called internally when rehydrating an aggregate.
        /// </summary>
        protected virtual void ApplyEvent(IEvent change)
        {
            if (State == null)
                State = CreateState();

            State.Apply(change);
        }
    }
}
