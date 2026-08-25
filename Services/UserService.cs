using Entities.Dtos;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IEncryptionService _encryptionService;

        public UserService(IRepositoryManager repositoryManager, IEncryptionService encryptionService)
        {
            _repositoryManager = repositoryManager;
            _encryptionService = encryptionService;
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

        public void RegisterUser(RegisterDto registerDto)
        {
            if (EmailExists(registerDto.Email))
            {
                throw new InvalidOperationException("Bu e-posta adresi başka bir kullanıcı tarafından kullanılmaktadır.");

            }

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                BirthDate = registerDto.BirthDate,
                EncryptedPassword = _encryptionService.Encrypt(registerDto.Password)
            };

            _repositoryManager.User.Create(user);
            _repositoryManager.Save();
        }

        public User? ValidateUser(LoginDto loginDto)
        {
            var user = GetUserByEmail(loginDto.Email, false);

            if (user is null)
            {
                return null;
            }

            var decryptedPassword = _encryptionService.Decrypt(user.EncryptedPassword);

            if (decryptedPassword != loginDto.Password)
            {
                return null;
            }

            return user;
        }

        public void UpdateProfile(UserProfileDto profileDto)
        {
            var user=GetUserById(profileDto.UserId,true);

            if(user is null)
            {
                throw new InvalidOperationException("Kullanıcı bulunamadı.");
            }

            if (EmailExists(profileDto.Email, profileDto.UserId))
            {
                throw new InvalidOperationException("Bu e-posta adresi başka bir kullanıcı tarafından kullanılmaktadır");
            }

            user.FirstName=profileDto.FirstName;
            user.LastName=profileDto.LastName;
            user.Email=profileDto.Email;
            user.BirthDate=profileDto.BirthDate;
            user.EncryptedPassword=_encryptionService.Encrypt(profileDto.Password);

            _repositoryManager.Save();
        }
    }
}