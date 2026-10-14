using MyBlog.Models;

namespace MyBlog.ViewModels
{
    public class CategoryWithCountViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PostsCount { get; set; }
    }

    public class HomeViewModel
    {
        public List<Post> LatestPosts { get; set; } = new();
        public List<CategoryWithCountViewModel> Categories { get; set; } = new();
    }
}