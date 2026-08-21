using Repositories.Contracts;

namespace Repositories
{

    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        public RepositoryManager(
            RepositoryContext repositoryContext,
            IUserRepository userRepository,
            IEventRepository eventRepository)
        {
            _repositoryContext = repositoryContext;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public IUserRepository User => _userRepository;

        public IEventRepository Event => _eventRepository;

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}