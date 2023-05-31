using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Command
{
    public class RenameUser : CommandBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public RenameUser(Guid id, string firstName, string lastName)
        {
            AggregateIdentifier = id;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
