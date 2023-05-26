using System;

namespace TStore.Shared.Entities
{
    public class Interaction
    {
        public enum ActionType
        {
            Click = 1,
            Search = 2,
            AccessPage = 3
        }

        public Guid Id { get; set; }
        public ActionType Action { get; set; }
        public string FromPage { get; set; }
        public string ToPage { get; set; }
        public string SearchTerm { get; set; }
        public int? ClickCount { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
