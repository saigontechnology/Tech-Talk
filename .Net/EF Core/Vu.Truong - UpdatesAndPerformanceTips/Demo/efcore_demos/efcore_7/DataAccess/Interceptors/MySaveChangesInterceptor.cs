namespace efcore_demos.DataAccess.Interceptors;
internal class MySaveChangesInterceptor : ISaveChangesInterceptor
{
    /// <summary>
    /// Called at the start of DbContext.SaveChanges.
    /// </summary>
    InterceptionResult<int> ISaveChangesInterceptor.SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        return result;
    }

    /// <summary>
    /// Called at the end of DbContext.SaveChanges
    /// </summary>
    int ISaveChangesInterceptor.SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        return result;
    }

    /// <summary>
    /// Called when an exception has been thrown in DbContext.SaveChanges.
    /// </summary>
    void ISaveChangesInterceptor.SaveChangesFailed(DbContextErrorEventData eventData)
    {

    }

    /// <summary>
    /// Called at the start of DbContext.SaveChangesAsync.
    /// </summary>
    ValueTask<InterceptionResult<int>> ISaveChangesInterceptor.SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken))
    {
        return ValueTask.FromResult(result);
    }

    /// <summary>
    /// Called at the end of DbContext.SaveChangesAsync.
    /// </summary>
    ValueTask<int> ISaveChangesInterceptor.SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default(CancellationToken))
    {
        return ValueTask.FromResult(result);
    }

    /// <summary>
    /// Called when an exception has been thrown in DbContext.SaveChangesAsync.
    /// </summary>
    Task ISaveChangesInterceptor.SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when DbContext.SaveChanges was canceled.
    /// </summary>
    void ISaveChangesInterceptor.SaveChangesCanceled(DbContextEventData eventData)
    {
    }

    /// <summary>
    ///  Called when DbContext.SaveChangesAsync was canceled.
    /// </summary>
    Task ISaveChangesInterceptor.SaveChangesCanceledAsync(DbContextEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called immediately before EF is going to throw a Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException.
    /// </summary>
    InterceptionResult ISaveChangesInterceptor.ThrowingConcurrencyException(ConcurrencyExceptionEventData eventData, InterceptionResult result)
    {
        return result;
    }

    /// <summary>
    /// Called immediately before EF is going to throw a Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException.
    /// </summary>
    ValueTask<InterceptionResult> ISaveChangesInterceptor.ThrowingConcurrencyExceptionAsync(ConcurrencyExceptionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken))
    {
        return new ValueTask<InterceptionResult>(result);
    }
}
