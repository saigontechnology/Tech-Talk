using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Query
{
    public class UserSummary
    {
        public Guid UserIdentifier { get; set; }
        public string Name { get; set; }
        public DateTimeOffset UserRegistered { get; set; }

        public int OpenAccountCount { get; set; }
        public decimal TotalAccountBalance { get; set; }

        public String LoginName { get; set; }
        public String LoginPassword { get; set; }
        public String UserRegistrationStatus { get; set; }
    }
}
