namespace testtask.Models
{
    public class PostDto
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }

        public List<CommentDto> comments { get; set; }
    }
}
