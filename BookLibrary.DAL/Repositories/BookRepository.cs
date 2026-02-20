using BookLibrary.DAL.Abstracts;
using BookLibrary.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _db;
        public BookRepository(AppDbContext db)
        {
            _db = db;
        }
        public int AddBook(Book book)
        {
            if (book.AuthorId.HasValue && !_db.Authors.Any(a => a.Id == book.AuthorId.Value))
            {
                return 0;
            }
            _db.Books.Add(book);
            _db.SaveChanges();
            return book.Id;
        }

        public Book GetBookById(int bookId)
        {
            return _db.Books.Include(b => b.Author).FirstOrDefault(x => x.Id == bookId);
        }

        public List<Book> GetAllBooks()
        {
            return _db.Books.Include(b => b.Author).ToList();
        }

        public bool UpdateBook(Book book)
        {
            var existingBook = _db.Books.FirstOrDefault(x => x.Id == book.Id);
            if (existingBook == null)
            {
                return false;
            }
            if (book.AuthorId.HasValue && !_db.Authors.Any(a => a.Id == book.AuthorId.Value))
            {
                return false;
            }
            existingBook.Title = book.Title;
            existingBook.ISBN = book.ISBN;
            existingBook.PublishYear = book.PublishYear;
            existingBook.Price = book.Price;
            existingBook.AuthorId = book.AuthorId;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteBook(int id)
        {
            var existingBook = _db.Books.FirstOrDefault(x => x.Id == id);
            if (existingBook == null)
            {
                return false;
            }

            try
            {
                _db.Books.Remove(existingBook);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
