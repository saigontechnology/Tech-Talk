

namespace BusinessLayer.Events
{
    public class TransferUpdated : EventBase
    {
        public TransferStatus Status => TransferStatus.Updated;
        public string Activity { get; set; }

        public TransferUpdated(string activity)
        {
            Activity = activity;
        }
    }
}
