using Destructurama.Attributed;

namespace BasicConsoleApp;

public class Payment
{
    public int PaymentId { get; set; }

    [LogMasked(ShowFirst = 3, PreserveLength = true)]
    public string? Email { get; set; }
    
    public Guid UserId { get; set; }

    public DateTime OccuredAt { get; set; }
}
