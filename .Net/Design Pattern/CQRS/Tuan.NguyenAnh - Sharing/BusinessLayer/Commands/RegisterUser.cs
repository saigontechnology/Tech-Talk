using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Command
{
    public class RegisterUser : CommandBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public RegisterUser(Guid id, string firstName, string lastName, string username, string password)
        {
            AggregateIdentifier = id;
            FirstName = firstName;
            LastName = lastName;

            UserName = username;
            Password = password;
        }
    }
}
