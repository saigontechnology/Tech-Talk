using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Command
{
    public class CompleteTransfer : CommandBase
    {
        public CompleteTransfer(Guid id)
        {
            AggregateIdentifier = id;
        }
    }
}
