using System.ComponentModel.DataAnnotations;

namespace MyBlog.ViewModels
{
    public class PostFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImagePath { get; set; }
        public List<int> SelectedTagIds { get; set; } = new();
    }
}