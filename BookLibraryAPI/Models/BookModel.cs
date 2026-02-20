namespace BookLibraryAPI.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public DateTime PublishYear { get; set; }
        public string Price { get; set; }
        public int? AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AuthorName { get; set; }
        public string? CoverUrl { get; set; }
        public string? CoverImageBase64 { get; set; }
    }
}
