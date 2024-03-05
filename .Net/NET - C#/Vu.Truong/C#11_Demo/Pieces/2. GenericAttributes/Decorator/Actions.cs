namespace C_11_Demo.Pieces.Decorator;
interface IAction
{
    void Action();
}

internal class Logger : IAction
{
    private IAction _actor;
    public Logger(IAction actor)
    {
        _actor = actor;
    }

    public void Action()
    {
        Console.WriteLine("Log before");
        _actor.Action();
        Console.WriteLine("Log after");
    }
}

internal class Counter : IAction
{
    private IAction _actor;

    public Counter(IAction actor)
    {
        _actor = actor;
    }

    public void Action()
    {
        Console.WriteLine("Count before");
        _actor.Action();
        Console.WriteLine("Count after");
    }
}

[Decorate<Counter>]
[Decorate<Logger>]
internal class Actor : IAction
{
    public void Action()
    {
        Console.WriteLine("Acting..");
    }
}

[Decorate<Logger>]
internal class ActorWithLog : IAction
{
    public void Action()
    {
        Console.WriteLine("Acting..");
    }
}

[Decorate<Counter>]
internal class ActorWithCounter : IAction
{
    public void Action()
    {
        Console.WriteLine("Acting..");
    }
}