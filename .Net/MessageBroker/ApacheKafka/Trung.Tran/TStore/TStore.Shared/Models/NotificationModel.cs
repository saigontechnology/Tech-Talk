namespace TStore.Shared.Models
{
    public class NotificationModel
    {
        public enum NotificationType
        {
            NewOrder = 1,
            PromotionApplied = 2,
            ShipApplied = 3,
            InteractionReportUpdated = 4,
            UnusualSearchs = 5,
            NewInteractions = 6,
            Log = 7
        }

        public NotificationType Type { get; set; }
        public object Data { get; set; }
    }
}
