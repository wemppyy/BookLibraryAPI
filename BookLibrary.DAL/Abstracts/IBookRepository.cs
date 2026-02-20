using BookLibrary.DAL.Models;

namespace BookLibrary.DAL.Abstracts
{
    public interface IBookRepository
    {
        int AddBook(Book book);
        bool UpdateBook(Book book);
        bool DeleteBook(int id);
        Book GetBookById(int bookId);
        List<Book> GetAllBooks();
    }
}
