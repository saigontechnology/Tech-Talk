using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Command
{
    public class CompleteUserRegistration : CommandBase
    {
        public string Status { get; set; }

        public CompleteUserRegistration(Guid id, string status)
        {
            AggregateIdentifier = id;
            Status = status;
        }
    }
}
