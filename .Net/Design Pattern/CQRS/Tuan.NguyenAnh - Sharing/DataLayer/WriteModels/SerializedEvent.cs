using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.WriteModels
{
    public class SerializedEvent : IEvent
    {
        public SerializedEvent()
        {
            EventTime = DateTimeOffset.UtcNow;
        }

        public Guid AggregateIdentifier { get; set; }
        public int AggregateVersion { get; set; }
        public DateTimeOffset EventTime { get; set; }

        public string EventClass { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
    }
}
