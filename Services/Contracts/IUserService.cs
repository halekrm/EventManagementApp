using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers(bool trackChanges);

        User? GetUserById(int id, bool trackChanges);
        User? GetUserByEmail(string email, bool trackChanges);
        User? ValidateUser(LoginDto loginDto);
        void CreateUser(User user);
        void UpdateUser(User user);
        void RegisterUser(RegisterDto registerDto);
        void UpdateProfile(UserProfileDto profileDto);
        bool EmailExists(string email, int? excludedUserId = null);
    }
}