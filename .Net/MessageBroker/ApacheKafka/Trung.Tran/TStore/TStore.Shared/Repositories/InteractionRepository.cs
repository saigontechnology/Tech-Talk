using TStore.Shared.Entities;
using TStore.Shared.Persistence;

namespace TStore.Shared.Repositories
{
    public interface IInteractionRepository : IBaseRepository<Interaction, IInteractionUnitOfWork>
    {
    }

    public class InteractionRepository : BaseRepository<Interaction, IInteractionUnitOfWork, InteractionContext>, IInteractionRepository
    {
        public InteractionRepository(InteractionContext dbContext, IInteractionUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }
    }
}
