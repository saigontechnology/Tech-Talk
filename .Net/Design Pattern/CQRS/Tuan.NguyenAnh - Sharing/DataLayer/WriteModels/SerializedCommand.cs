using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.WriteModels
{
    public class SerializedCommand : ICommand
    {
        public Guid AggregateIdentifier { get; set; }

        public string CommandClass { get; set; }
        public string CommandType { get; set; }
        public string CommandData { get; set; }

        public Guid CommandIdentifier { get; set; }

        public DateTimeOffset? SendStarted { get; set; }
        public DateTimeOffset? SendCompleted { get; set; }

        public string SendStatus { get; set; }
    }
}
