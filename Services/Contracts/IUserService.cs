using Entities.Models;

namespace Services.Contracts
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers(bool trackChanges);

        User? GetUserById(int id, bool trackChanges);
        User? GetUserByEmail(string email, bool trackChanges);
        void CreateUser(User user);
        void UpdateUser(User user);
        bool EmailExists(string email, int? excludedUserId = null);
    }
}