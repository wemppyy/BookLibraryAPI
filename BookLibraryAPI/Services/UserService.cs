using BookLibrary.DAL.Abstracts;
using BookLibrary.DAL.Models;
using BookLibraryAPI.Abstracts;
using BookLibraryAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace BookLibraryAPI.Services
{
    public class UserService : IUserSerivce
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool Create(RegisterRequest request)
        {
            return Create(request, Roles.User);
        }

        public bool Create(RegisterRequest request, string role)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return false;
            }

            var existingUser = _userRepository.FindUser(request.Email);
            if (existingUser != null)
            {
                return false;
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = Hash(request.Password),
                Role = role
            };

            return _userRepository.CreateUser(user);
        }

        public UserRecord Find(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var user = _userRepository.FindUser(email);
            if (user == null)
            {
                return null;
            }

            return new UserRecord
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.Role
            };
        }

        public IEnumerable<UserRecord> GetAll()
        {
            var users = _userRepository.GetAllUsers();
            return users.Select(u => new UserRecord
            {
                Id = u.Id.ToString(),
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Role = u.Role
            }).ToList();
        }

        public bool CheckPassword(UserRecord user, string password)
        {
            if (user == null || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var passwordHash = Hash(password);
            var dbUser = new User
            {
                Id = int.Parse(user.Id),
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.Role
            };

            return _userRepository.CheckPassword(dbUser, passwordHash);
        }

        public string Hash(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
