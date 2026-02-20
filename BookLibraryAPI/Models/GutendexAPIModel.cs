namespace BookLibraryAPI.Models
{
    public class GutendexAPIModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int? BirthYear { get; set; }
        public string ImageUrl { get; set; }
    }

    public class GutendexPagedResult
    {
        public List<GutendexAPIModel> Books { get; set; }
        public int? NextPage { get; set; }
        public int? PreviousPage { get; set; }
    }
}
