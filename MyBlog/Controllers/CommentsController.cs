using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.ViewModels;

namespace MyBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly AppDbContext _context;
        public CommentsController(AppDbContext context)
        {
            _context = context;
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentFormViewModel model)
        {
            if (!await _context.Posts.AnyAsync(p => p.Id == model.PostId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(p => p.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join(" | ", errors);
                return RedirectToAction("Details", "Post", new { id = model.PostId }, "comments");
            }
            var comment = new Comment
            {
                PostId = model.PostId,
                AuthorName = model.AuthorName,
                Text = model.Text
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Comment added successfully";
            return RedirectToAction("Details", "Posts", new { id = model.PostId }, "comments");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Posts.FindAsync(id);
            if (comment == null) return NotFound();
            var postId = comment.Id;
            _context.Remove(comment);
            _context.SaveChangesAsync();
            TempData["Success"] = "Comment deleted";
            return RedirectToAction("Details", "Posts", new { id = postId }, "comments");

        }
    }
}