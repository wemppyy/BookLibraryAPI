using BookLibrary.DAL.Abstracts;
using BookLibrary.DAL.Models;
using BookLibraryAPI.Abstracts;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public int Add(AuthorRequest authorRequest)
        {
            var author = new Author
            {
                FirstName = authorRequest.FirstName,
                LastName = authorRequest.LastName,
                DateOfBirth = authorRequest.DateOfBirth,
                CreatedAt = DateTime.Now
            };
            return _authorRepository.AddAuthor(author);
        }

        public bool Delete(int id)
        {
            return _authorRepository.DeleteAuthor(id);
        }

        public List<AuthorModel> GetAll()
        {
            var authors = _authorRepository.GetAllAuthors();
            var authorModels = authors.Select(a => new AuthorModel
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                DateOfBirth = a.DateOfBirth,
                CreatedAt = a.CreatedAt
            }).ToList();
            return authorModels;
        }

        public AuthorModel GetById(int id)
        {
            var author = _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return null;
            }
            var authorModel = new AuthorModel
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateOfBirth = author.DateOfBirth
            };
            return authorModel;
        }

        public bool Update(AuthorRequest authorRequest, int id)
        {
            var existingAuthor = _authorRepository.GetAuthorById(id);
            if (existingAuthor == null)
            {
                return false;
            }
            existingAuthor.FirstName = authorRequest.FirstName;
            existingAuthor.LastName = authorRequest.LastName;
            existingAuthor.DateOfBirth = authorRequest.DateOfBirth;
            return _authorRepository.UpdateAuthor(existingAuthor);
        }
    }
}
