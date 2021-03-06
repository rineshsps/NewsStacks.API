using NewsStacks.DTOs.Enum;

namespace NewsStacks.DTOs
{
    public class ArticleMessage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public MessageType MessageType { get; set; }
    }
}

