using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.WriteModels
{
    public class Snapshot
    {
        public Guid AggregateIdentifier { get; set; }
        public int AggregateVersion { get; set; }
        public string AggregateState { get; set; }
    }
}
