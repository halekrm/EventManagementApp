using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{

    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
