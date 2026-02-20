using BookLibrary.DAL.Models;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Abstracts
{
    public interface IBookService
    {
        int Add(BookRequest bookRequest);
        bool Update(BookRequest bookRequest, int id);
        bool Delete(int id);
        BookModel GetById(int id);
        List<BookModel> GetAll();
    }
}
