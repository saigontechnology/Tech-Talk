using System;

namespace BusinessLayer.Events
{
    public class UserRegistered : EventBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset Registered { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }

        public UserRegistered(string first, string last, DateTimeOffset registered, string username, string pass, string status)
        {
            FirstName = first;
            LastName = last;
            Registered = registered;
            Name = username;
            Password = pass;
            Status = status;
        }
    }
}
