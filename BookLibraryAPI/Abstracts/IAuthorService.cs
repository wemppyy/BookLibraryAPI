using BookLibrary.DAL.Models;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Abstracts
{
    public interface IAuthorService
    {
        int Add(AuthorRequest authorRequest);
        bool Update(AuthorRequest authorRequest, int id);
        bool Delete(int id);
        List<AuthorModel> GetAll();
        AuthorModel GetById(int id);
    }
}
