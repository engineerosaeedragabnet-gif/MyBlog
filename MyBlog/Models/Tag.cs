using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Tag
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tag name is required")]
        [MaxLength(50, ErrorMessage = "Tag name cannot exceed 50 characters")]
        [Display(Name = "Tag Name")]
        public string Name { get; set; } = string.Empty;

        public List<Post> Posts { get; set; } = new();
    }
}