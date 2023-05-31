using BusinessLayer.Events;
using DataLayer.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Aggregates
{
    public class UserAggregateState : AggregateState
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
        public DateTimeOffset Registered { get; set; }

        public void When(UserRegistered @event)
        {
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            Name = @event.Name;
            PassWord = @event.Password;
            Registered = @event.Registered;
            Status = @event.Status;
        }

        public void When(UserRenamed @event)
        {
            FirstName = @event.FirstName;
            LastName = @event.LastName;
        }
    }
}
