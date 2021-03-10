using System.ComponentModel.DataAnnotations;

namespace NewsStacks.DTOs
{
    public class ArticleUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2), MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MinLength(2)]
        public string Description { get; set; }
        [Required]
        [MinLength(2)]
        public string Tags { get; set; }
        [Required]
        [MinLength(2), MaxLength(25)]
        public string Topics { get; set; }
        public bool IsDraft { get; set; }
        [Required]
        [MinLength(2), MaxLength(250)]
        public string ReviewerComments { get; set; }
        [Required]
        [MinLength(2), MaxLength(250)]
        public string EditorComments { get; set; }
    }
}
