using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IEvent
    {
        Guid AggregateIdentifier { get; set; }
        int AggregateVersion { get; set; }

        DateTimeOffset EventTime { get; set; }
    }
}
