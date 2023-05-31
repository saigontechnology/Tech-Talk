using DataLayer.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Helper
{
    public static class EventExtensions
    {
        /// <summary>
        /// Returns a deserialized event.
        /// </summary>
        public static IEvent Deserialize(this SerializedEvent x, ISerializer serializer)
        {
            var data = serializer.Deserialize<IEvent>(x.EventData, Type.GetType(x.EventClass));

            data.AggregateIdentifier = x.AggregateIdentifier;
            data.AggregateVersion = x.AggregateVersion;
            data.EventTime = x.EventTime;

            return data;
        }

        /// <summary>
        /// Returns a serialized event.
        /// </summary>
        public static SerializedEvent Serialize(this IEvent @event, ISerializer serializer, Guid aggregateIdentifier, int version)
        {
            var data = serializer.Serialize(@event, new[] { "AggregateIdentifier", "AggregateVersion", "EventTime", "IdentityTenant", "IdentityUser" });

            var serialized = new SerializedEvent
            {
                AggregateIdentifier = aggregateIdentifier,
                AggregateVersion = version,

                EventTime = @event.EventTime,
                EventClass = @event.GetType().AssemblyQualifiedName,
                EventType = @event.GetType().Name,
                EventData = data,
            }; 

            return serialized;
        }
    }
}
