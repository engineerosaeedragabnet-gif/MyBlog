using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required,MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedAt {  get; set; }= DateTime.Now;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Tag> Tags { get; set; }
    }
}
