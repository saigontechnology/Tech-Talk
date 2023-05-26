using System;

namespace TStore.Shared.Models
{
    public class UnusualSearchModel
    {
        public string SearchTerm { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
