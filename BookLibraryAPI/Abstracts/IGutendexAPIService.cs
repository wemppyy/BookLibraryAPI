using BookLibraryAPI.Models;

namespace BookLibraryAPI.Abstracts
{
    public interface IGutendexAPIService
    {
        Task<GutendexPagedResult> GetAllBooks(int page = 1, string search = null);
        Task<GutendexAPIModel> GetById(int id);
    }
}
