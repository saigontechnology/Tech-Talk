using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.RequestDto
{
    public class OpenAccountDto
    {
        public Guid Owner { get; set; }
        public string Code { get; set; }

        public Decimal Balance { get; set; }
    }
}
