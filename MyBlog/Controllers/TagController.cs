using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.ViewModels;

namespace MyBlog.Controllers
{
    public class TagsController : Controller
    {
        private readonly AppDbContext _context;

        public TagsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Tags
        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags
                .OrderBy(t => t.Name)
                .Select(t => new TagListItemViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    PostsCount = t.Posts.Count
                })
                .ToListAsync();

            return View(tags);
        }

        // GET: Tags/Create
        public IActionResult Create() => View();

        // POST: Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(nameof(Tag.Name))] Tag tag)
        {
            tag.Name = tag.Name?.Trim() ?? string.Empty;

            if (ModelState.IsValid && await _context.Tags.AnyAsync(t => t.Name == tag.Name))
                ModelState.AddModelError(nameof(Tag.Name), "This tag already exists");

            if (!ModelState.IsValid)
                return View(tag);

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Tag created successfully";
            return RedirectToAction(nameof(Index));
        }

        // POST: Tags/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Tag deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}