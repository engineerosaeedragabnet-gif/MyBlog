namespace MyBlog.ViewModels
{
    public class TagListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PostsCount { get; set; }
    }
}