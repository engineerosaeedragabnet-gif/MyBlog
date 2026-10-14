using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
