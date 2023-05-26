using System;

namespace TStore.Shared.Entities
{
    public class InteractionReport
    {
        public Guid Id { get; set; }
        public Interaction.ActionType Action { get; set; }
        public int Count { get; set; }
    }
}
