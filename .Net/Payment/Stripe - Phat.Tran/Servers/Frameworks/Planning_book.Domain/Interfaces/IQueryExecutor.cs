namespace PlanningBook.Domain.Interfaces
{
    public interface IQueryExecutor
    {
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}
