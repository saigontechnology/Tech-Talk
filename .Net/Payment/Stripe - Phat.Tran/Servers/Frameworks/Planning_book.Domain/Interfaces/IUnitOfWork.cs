namespace PlanningBook.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        //TODO: BulkSaveChange
    }
}
