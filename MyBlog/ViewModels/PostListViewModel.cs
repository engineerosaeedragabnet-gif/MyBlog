using MyBlog.Models;

namespace MyBlog.ViewModels
{
    public class PostListViewModel
    {
        public List<Post> Posts { get; set; } = new();

        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public string SortOrder { get; set; } = "newest";


        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int? TagId { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}