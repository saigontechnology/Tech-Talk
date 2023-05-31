using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Events
{
    public class AccountClosed : EventBase
    {
        public string Reason { get; set; }

        public AccountClosed(string reason)
        {
            Reason = reason;
        }
    }
}
