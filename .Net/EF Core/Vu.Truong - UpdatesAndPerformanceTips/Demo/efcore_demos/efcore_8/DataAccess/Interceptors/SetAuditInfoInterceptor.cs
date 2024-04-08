using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace efcore_demos.DataAccess.Interceptors;

internal class SetAuditInfoInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null && eventData.Context is IAuditDbContext)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        Console.WriteLine("SavingChangesAsync Interceptor");

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext dbContext)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var entities = dbContext.ChangeTracker.Entries<IBaseEntity>().ToList();

        foreach (EntityEntry<IBaseEntity> entry in entities)
        {
            if (entry.State == EntityState.Added)
            {
                SetCurrentPropertyValue(entry, nameof(IBaseEntity.CreatedDate), now);
            }

            if (entry.State == EntityState.Modified)
            {
                SetCurrentPropertyValue(entry, nameof(IBaseEntity.UpdatedDate), now);
            }
        }
    }

    static void SetCurrentPropertyValue(EntityEntry entry, string propertyName, DateTimeOffset dateTime)
    {
        entry.Property(propertyName).CurrentValue = dateTime;
    }
}