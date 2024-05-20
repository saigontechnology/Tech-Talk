namespace AdvancedTechniques;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeNow => DateTime.Now;
}

public interface IDateTimeProvider
{
    public DateTime DateTimeNow { get; }
}
