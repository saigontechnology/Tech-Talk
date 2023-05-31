using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.WriteModels
{
    public class SerializedAggregate
    {
        public string AggregateClass { get; set; }
        public DateTimeOffset? AggregateExpires { get; set; }
        public Guid AggregateIdentifier { get; set; }
        public string AggregateType { get; set; }
    }
}
