

namespace BusinessLayer.Events
{
    public class UserRenamed : EventBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserRenamed(string first, string last) { FirstName = first; LastName = last; }
    }
}
