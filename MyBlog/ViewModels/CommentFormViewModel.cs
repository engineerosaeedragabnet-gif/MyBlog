using System.ComponentModel.DataAnnotations;

namespace MyBlog.ViewModels
{
    public class CommentFormViewModel
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string AuthorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comment text is required")]
        [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string Text { get; set; } = string.Empty;

    }
}
