using System;
using TStore.Shared.Entities;

namespace TStore.Shared.Models
{
    public class InteractionModel
    {
        public Guid Id { get; set; }
        public Interaction.ActionType Action { get; set; }
        public string FromPage { get; set; }
        public string ToPage { get; set; }
        public string SearchTerm { get; set; }
        public int? ClickCount { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
