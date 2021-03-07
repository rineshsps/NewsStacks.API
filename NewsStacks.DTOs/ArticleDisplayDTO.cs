using System;

namespace NewsStacks.DTOs
{
    public class ArticleDisplayDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Topics { get; set; }
        public bool IsDraft { get; set; }
        public bool Active { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int Likes { get; set; }
    }
}
