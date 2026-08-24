using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;

        public UserService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public IEnumerable<User> GetAllUsers(bool trackChanges)
        {
            return _repositoryManager.User
                    .FindAll(trackChanges)
                    .ToList();
        }
        public User? GetUserById(int id, bool trackChanges)
        {
            return _repositoryManager.User
                    .FindByCondition(user => user.UserId == id, trackChanges)
                    .SingleOrDefault();
        }
        public User? GetUserByEmail(string email, bool trackChanges)
        {
            return _repositoryManager.User
                   .FindByCondition(user => user.Email == email, trackChanges)
                   .SingleOrDefault();

        }


        public void CreateUser(User user)
        {
            if (EmailExists(user.Email))
            {
                throw new InvalidOperationException("Bu e-posta adresi ile kayıtlı bir kullanıcı bulunmaktadır.");
            }
            _repositoryManager.User.Create(user);
            _repositoryManager.Save();
        }
        public void UpdateUser(User user)
        {
            if (EmailExists(user.Email, user.UserId))
            {
                throw new InvalidOperationException("Bu e-posta adresi başkası tarafından kullanılmaktadır.");
            }
            _repositoryManager.User.Update(user);
            _repositoryManager.Save();
        }
        public bool EmailExists(string email, int? excludedUserId = null)
        {
            return _repositoryManager.User.FindByCondition(user =>
                    user.Email == email &&
                    (!excludedUserId.HasValue ||
                    user.UserId != excludedUserId.Value), false)
                    .Any();
        }

    }
}