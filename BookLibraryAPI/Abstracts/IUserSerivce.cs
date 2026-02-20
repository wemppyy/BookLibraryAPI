using BookLibraryAPI.Models;

namespace BookLibraryAPI.Abstracts
{
    public interface IUserSerivce
    {
        bool Create(RegisterRequest request);
        bool Create(RegisterRequest request, string role);
        UserRecord Find(string email);
        IEnumerable<UserRecord> GetAll();
        bool CheckPassword(UserRecord user, string password);
        string Hash(string password);
    }
}
