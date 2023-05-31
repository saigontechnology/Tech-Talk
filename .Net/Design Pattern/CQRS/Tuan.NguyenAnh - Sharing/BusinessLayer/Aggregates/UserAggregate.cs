using BusinessLayer.Events;
using DataLayer.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Aggregates
{
    public class UserAggregate : AggregateRoot
    {
        public override AggregateState CreateState() => new UserAggregateState();


        public void RegisterUser(string firstName, string lastName, DateTimeOffset registered,string username, string pass, string status)
        {
            var e = new UserRegistered(firstName, lastName, registered, username, pass, status);
            Apply(e);
        }

        public void RenameUser(string firstName, string lastName)
        {
            var e = new UserRenamed(firstName, lastName);
            Apply(e);
        }
    }
}
