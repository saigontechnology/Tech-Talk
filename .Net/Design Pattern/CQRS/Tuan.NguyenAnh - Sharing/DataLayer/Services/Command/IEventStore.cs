using DataLayer.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Command
{
    /// <summary>
    /// Defines the methods needed from the event store.
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Utility for serializing and deserializing events.
        /// </summary>
        ISerializer Serializer { get; }

        /// <summary>
        /// Returns true if an aggregate exists.
        /// </summary>
        bool Exists(Guid aggregate);

        /// <summary>
        /// Returns true if an aggregate with a specific version exists.
        /// </summary>
        bool Exists(Guid aggregate, int version);

        /// <summary>
        /// Gets events for an aggregate starting at a specific version. To get all events use version = -1.
        /// </summary>
        IEnumerable<IEvent> Get(Guid aggregate, int version, string aggregateType);

        /// <summary>
        /// Save events.
        /// </summary>
        void Save(AggregateRoot aggregate, IEnumerable<IEvent> events);

    }
}
