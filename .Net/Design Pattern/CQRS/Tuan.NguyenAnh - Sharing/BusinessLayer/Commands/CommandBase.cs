using DataLayer;
using System.Runtime.Serialization;

namespace BusinessLayer.Command
{
    public class CommandBase : ICommand
    {
        [IgnoreDataMember]
        public Guid AggregateIdentifier { get; set; }
        [IgnoreDataMember]
        public int? ExpectedVersion { get; set; }
        [IgnoreDataMember]
        public Guid CommandIdentifier { get; set; }
        public CommandBase() { CommandIdentifier = Guid.NewGuid(); }
    }
}
