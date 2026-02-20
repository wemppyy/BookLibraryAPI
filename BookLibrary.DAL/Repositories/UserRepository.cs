using BookLibrary.DAL.Abstracts;
using BookLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BookLibrary.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public bool CheckPassword(User user, string passwordHash)
        {
            if (user == null || string.IsNullOrEmpty(passwordHash))
            {
                return false;
            }
            return user.PasswordHash == passwordHash;
        }

        public bool CreateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash) || string.IsNullOrEmpty(user.Role))
            {
                return false;
            }

            var res = _db.Users.Add(user) != null;
            _db.SaveChanges();
            return res;
        }

        public User FindUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users.ToList();
        }
    }
}
