using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}