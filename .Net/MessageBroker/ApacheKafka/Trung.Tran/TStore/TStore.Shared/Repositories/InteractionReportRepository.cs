using TStore.Shared.Entities;
using TStore.Shared.Persistence;

namespace TStore.Shared.Repositories
{
    public interface IInteractionReportRepository : IBaseRepository<InteractionReport, IInteractionUnitOfWork>
    {
    }

    public class InteractionReportRepository : BaseRepository<InteractionReport, IInteractionUnitOfWork, InteractionContext>, IInteractionReportRepository
    {
        public InteractionReportRepository(InteractionContext dbContext, IInteractionUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }
    }
}
