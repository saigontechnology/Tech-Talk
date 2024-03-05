namespace NetSevenLib.Contracts;
public interface IExecFunction
{
    static abstract void Execute();

    static abstract int Order { get; }
    static abstract string Name { get; }
    static abstract string Description { get; }
}