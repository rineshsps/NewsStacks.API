using System.ComponentModel.DataAnnotations;

namespace NewsStacks.DTOs
{
    public class ArticleDTO
    {
        [Required]
        [MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MinLength(2)]
        public string Description { get; set; }
        [Required]
        [MinLength(2), MaxLength(25)]
        public string Topics { get; set; }
        [Required]
        [MinLength(2)]
        public string Tags { get; set; }
        public bool IsDraft { get; set; }
    }
}
