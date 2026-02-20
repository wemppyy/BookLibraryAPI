using BookLibrary.DAL.Abstracts;
using BookLibrary.DAL.Models;

namespace BookLibrary.DAL.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _db;
        public AuthorRepository(AppDbContext db)
        {
            _db = db;
        }
        public int AddAuthor(Author author)
        {
            _db.Authors.Add(author);
            _db.SaveChanges();
            return author.Id;
        }

        public bool UpdateAuthor(Author author)
        {
            var existingAuthor = _db.Authors.FirstOrDefault(x => x.Id == author.Id);
            if (existingAuthor == null)
            {
                return false;
            }
            existingAuthor.FirstName = author.FirstName;
            existingAuthor.LastName = author.LastName;
            existingAuthor.DateOfBirth = author.DateOfBirth;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteAuthor(int id)
        {
            var existingAuthor = _db.Authors.FirstOrDefault(x => x.Id == id);
            if (existingAuthor == null)
            {
                return false;
            }

            try
            {
                _db.Authors.Remove(existingAuthor);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Author GetAuthorById(int authorId)
        {
            return _db.Authors.FirstOrDefault(x => x.Id == authorId);
        }

        public List<Author> GetAllAuthors()
        {
            return _db.Authors.ToList();
        }
    }
}
