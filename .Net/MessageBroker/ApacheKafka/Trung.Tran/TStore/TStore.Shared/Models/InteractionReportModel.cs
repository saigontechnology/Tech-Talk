using System;
using TStore.Shared.Entities;

namespace TStore.Shared.Models
{
    public class InteractionReportModel
    {
        public Guid Id { get; set; }
        public Interaction.ActionType Action { get; set; }
        public int Count { get; set; }
    }
}
