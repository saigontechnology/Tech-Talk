namespace C_11_Demo.Pieces;
public interface IOperators<TSelf>
        where TSelf : IOperators<TSelf>?
{
    static abstract TSelf operator +(TSelf left, TSelf right);

    static abstract IOperators<TSelf> Default { get; }
}

public class FirstOperator : IOperators<FirstOperator>
{
    public int Value { get; set; } = 0;
    private static IOperators<FirstOperator> _default;
    public static IOperators<FirstOperator> Default => _default ??= new FirstOperator();

    public static FirstOperator operator +(FirstOperator left, FirstOperator right)
    {
        return new FirstOperator
        {
            Value = left.Value + right.Value
        };
    }
}