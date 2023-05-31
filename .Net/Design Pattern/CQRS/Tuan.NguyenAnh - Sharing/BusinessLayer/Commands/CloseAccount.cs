using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Command
{
    public class CloseAccount : CommandBase
    {
        public string Reason { get; set; }

        public CloseAccount(Guid account, string reason)
        {
            AggregateIdentifier = account;
            Reason = reason;
        }
    }
}
