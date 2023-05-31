using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Command
{
    public class OpenAccount : CommandBase
    {
        public Guid Owner { get; set; }
        public string Code { get; set; }

        public OpenAccount(Guid id, Guid owner, string code)
        {
            AggregateIdentifier = id;
            Owner = owner;
            Code = code;
        }
    }
}
