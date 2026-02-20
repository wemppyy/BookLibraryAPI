using BookLibrary.DAL.Models;

namespace BookLibrary.DAL.Abstracts
{
    public interface IAuthorRepository
    {
        Author GetAuthorById(int authorId);
        int AddAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(int id);
        List<Author> GetAllAuthors();
    }
}
