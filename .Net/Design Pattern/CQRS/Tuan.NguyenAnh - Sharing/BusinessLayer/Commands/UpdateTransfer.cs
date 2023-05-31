using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Command
{
    public class UpdateTransfer : CommandBase
    {
        public string Activity { get; set; }

        public UpdateTransfer(Guid id, string activity)
        {
            AggregateIdentifier = id;
            Activity = activity;
        }
    }
}
