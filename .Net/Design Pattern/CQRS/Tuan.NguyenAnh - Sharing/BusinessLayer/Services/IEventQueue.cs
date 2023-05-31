using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Serivces
{
    /// <summary>
    /// Provides the features for a basic service bus to handle the publication of events.
    /// </summary>
    public interface IEventQueue
    {
        /// <summary>
        /// Publishes an event to registered subscribers.
        /// </summary>
        void Publish(IEvent @event);

        /// <summary>
        /// Registers a handler for a specific event.
        /// </summary>
        void Subscribe<T>(Action<T> action) where T : IEvent;
    }
}
