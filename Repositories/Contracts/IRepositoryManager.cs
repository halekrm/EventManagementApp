namespace Repositories.Contracts
{

    public interface IRepositoryManager
    {
        IUserRepository User { get; }

        IEventRepository Event { get; }

        void Save();
    }
}