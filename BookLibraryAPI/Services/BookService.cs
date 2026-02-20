using BookLibrary.DAL.Abstracts;
using BookLibrary.DAL.Models;
using BookLibraryAPI.Abstracts;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public int Add(BookRequest bookRequest)
        {
            if (bookRequest.AuthorId.HasValue && bookRequest.AuthorId.Value != 0 && _authorRepository.GetAuthorById(bookRequest.AuthorId.Value) == null)
            {
                return 0;
            }

            var book = new Book
            {
                Title = bookRequest.Title,
                ISBN = bookRequest.ISBN,
                PublishYear = bookRequest.PublishYear,
                Price = bookRequest.Price,
                AuthorId = bookRequest.AuthorId == 0 ? null : bookRequest.AuthorId,
                CoverUrl = bookRequest.CoverUrl,
                CoverImageBase64 = bookRequest.CoverImageBase64,
                CreatedAt = DateTime.Now
            };
            return _bookRepository.AddBook(book);
        }

        public bool Delete(int id)
        {
            return _bookRepository.DeleteBook(id);
        }

        public List<BookModel> GetAll()
        {
            var books = _bookRepository.GetAllBooks();
            var bookModels = books.Select(b => new BookModel
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublishYear = b.PublishYear,
                Price = b.Price,
                AuthorId = b.AuthorId,
                CreatedAt = b.CreatedAt,
                AuthorName = b.Author != null ? b.Author.FirstName + " " + b.Author.LastName : null,
                CoverUrl = b.CoverUrl,
                CoverImageBase64 = b.CoverImageBase64
            }).ToList();
            return bookModels;
        }

        public BookModel GetById(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return null;
            }
            var bookModel = new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishYear = book.PublishYear,
                Price = book.Price,
                AuthorId = book.AuthorId,
                CreatedAt = book.CreatedAt,
                AuthorName = book.Author != null ? book.Author.FirstName + " " + book.Author.LastName : null,
                CoverUrl = book.CoverUrl,
                CoverImageBase64 = book.CoverImageBase64
            };
            return bookModel;
        }

        public bool Update(BookRequest bookRequest, int id)
        {
            var existingBook = _bookRepository.GetBookById(id);
            if (existingBook == null)
            {
                return false;
            }
            if (bookRequest.AuthorId.HasValue && bookRequest.AuthorId.Value != 0 && _authorRepository.GetAuthorById(bookRequest.AuthorId.Value) == null)
            {
                return false;
            }

            existingBook.Title = bookRequest.Title;
            existingBook.ISBN = bookRequest.ISBN;
            existingBook.PublishYear = bookRequest.PublishYear;
            existingBook.Price = bookRequest.Price;
            existingBook.AuthorId = bookRequest.AuthorId == 0 ? null : bookRequest.AuthorId;
            existingBook.CoverUrl = bookRequest.CoverUrl;
            existingBook.CoverImageBase64 = bookRequest.CoverImageBase64;
            return _bookRepository.UpdateBook(existingBook);
        }
    }
}
