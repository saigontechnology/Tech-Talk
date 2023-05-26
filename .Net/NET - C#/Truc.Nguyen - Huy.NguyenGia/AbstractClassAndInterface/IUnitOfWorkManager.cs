namespace AbstractClassAndInterface
{
    public interface IUnitOfWorkManager
    {
        IActiveUnitOfWork Current { get; }

    }

    public interface IActiveUnitOfWork
    {

    }
}