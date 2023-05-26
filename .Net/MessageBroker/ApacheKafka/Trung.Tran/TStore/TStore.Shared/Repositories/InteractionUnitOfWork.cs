using TStore.Shared.Persistence;

namespace TStore.Shared.Repositories
{
    public interface IInteractionUnitOfWork : IUnitOfWork
    {
    }

    public class InteractionUnitOfWork : BaseUnitOfWork<InteractionContext>, IInteractionUnitOfWork
    {
        public InteractionUnitOfWork(InteractionContext dbContext) : base(dbContext)
        {
        }
    }
}
