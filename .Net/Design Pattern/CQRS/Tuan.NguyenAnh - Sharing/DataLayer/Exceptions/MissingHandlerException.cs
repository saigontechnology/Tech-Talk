using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Exceptions
{
    public class MissingHandlerException : Exception
    {
        public MissingHandlerException(string message) : base(message) { }
    }
}
