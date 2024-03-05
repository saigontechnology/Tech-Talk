namespace C_11_Demo.Contracts;
public interface IExample
{
    static abstract void Execute();

    static abstract int Order { get; }
    static abstract string Name { get; }
    static abstract string Description { get; }
}
