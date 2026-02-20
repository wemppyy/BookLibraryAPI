using BookLibrary.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookLibrary.DAL.Abstracts
{
    public interface IUserRepository
    {
        bool CreateUser(User user);
        User FindUser(string email);
        IEnumerable<User> GetAllUsers();
        bool CheckPassword(User user, string passwordHash);
    }
}
