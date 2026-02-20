using System;
using System.Collections.Generic;
using System.Text;

namespace BookLibrary.DAL.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public DateTime PublishYear { get; set; }
        public string Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CoverUrl { get; set; }
        public string? CoverImageBase64 { get; set; }

        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
