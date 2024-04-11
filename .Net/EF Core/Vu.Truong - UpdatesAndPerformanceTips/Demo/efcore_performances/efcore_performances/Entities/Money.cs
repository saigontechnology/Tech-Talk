namespace efcore_performances.Entities;
public record struct Money(decimal Amount, Currency Currency)
{
    public override string ToString()
        => (Currency == Currency.UsDollars ? "$" : "£") + Amount;
}

