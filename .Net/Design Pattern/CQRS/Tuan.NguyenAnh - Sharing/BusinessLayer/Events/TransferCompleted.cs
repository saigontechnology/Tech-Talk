

namespace BusinessLayer.Events
{
    public class TransferCompleted : EventBase
    {
        public TransferStatus Status => TransferStatus.Completed;

        public TransferCompleted()
        {

        }
    }
}
