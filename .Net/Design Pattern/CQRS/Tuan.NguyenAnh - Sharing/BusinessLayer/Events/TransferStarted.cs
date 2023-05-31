
namespace BusinessLayer.Events
{
    public class TransferStarted : EventBase
    {
        public TransferStatus Status => TransferStatus.Started;
        public Guid FromAccount { get; set; }
        public Guid ToAccount { get; set; }
        public decimal Amount { get; set; }

        public TransferStarted(Guid fromAccount, Guid toAccount, decimal amount)
        {
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Amount = amount;
        }
    }

    public enum TransferStatus { Started, Updated, Completed }
}
