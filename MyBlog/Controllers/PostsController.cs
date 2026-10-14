using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.ViewModels;

namespace MyBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB

        public PostsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Posts
        // GET: Posts
        // GET: Posts
        public async Task<IActionResult> Index(string? searchTerm, int? categoryId, int? tagId,
                                               string sortOrder = "newest", int page = 1)
        {
            const int pageSize = 6;

            IQueryable<Post> query = _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(p => p.Title.Contains(term) || p.Content.Contains(term));
            }

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (tagId.HasValue)
                query = query.Where(p => p.Tags.Any(t => t.Id == tagId.Value));

            var totalItems = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = Math.Clamp(page, 1, Math.Max(1, totalPages));

            query = sortOrder switch
            {
                "oldest" => query.OrderBy(p => p.CreatedAt),
                "title" => query.OrderBy(p => p.Title),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            var posts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (tagId.HasValue)
            {
                ViewBag.ActiveTagName = await _context.Tags
                    .Where(t => t.Id == tagId.Value)
                    .Select(t => t.Name)
                    .FirstOrDefaultAsync();
            }

            var vm = new PostListViewModel
            {
                Posts = posts,
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                TagId = tagId,
                SortOrder = sortOrder,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            await PopulateCategories(categoryId);
            return View(vm);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt))
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();
            return View(post);
        }

        // GET: Posts/Create
        // GET: Posts/Create
        public async Task<IActionResult> Create()
        {
            await PopulateCategories();
            await PopulateTags();
            return View(new PostFormViewModel());
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostFormViewModel vm)
        {
            ValidateImage(vm.ImageFile, nameof(vm.ImageFile));

            if (!ModelState.IsValid)
            {
                await PopulateCategories(vm.CategoryId);
                await PopulateTags();
                return View(vm);
            }

            var post = new Post
            {
                Title = vm.Title,
                Content = vm.Content,
                CategoryId = vm.CategoryId
            };

            if (vm.SelectedTagIds.Any())
            {
                post.Tags = await _context.Tags
                    .Where(t => vm.SelectedTagIds.Contains(t.Id))
                    .ToListAsync();
            }

            if (vm.ImageFile != null)
                post.ImagePath = await SaveImageAsync(vm.ImageFile);

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // FindAsync مبيدعمش Include، فبنستخدم FirstOrDefaultAsync
            var post = await _context.Posts
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            var vm = new PostFormViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CategoryId = post.CategoryId,
                ExistingImagePath = post.ImagePath,
                SelectedTagIds = post.Tags.Select(t => t.Id).ToList()
            };

            await PopulateCategories(post.CategoryId);
            await PopulateTags();
            return View(vm);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            var post = await _context.Posts
                .Include(p => p.Tags)          
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            ValidateImage(vm.ImageFile, nameof(vm.ImageFile));

            if (!ModelState.IsValid)
            {
                vm.ExistingImagePath = post.ImagePath;
                await PopulateCategories(vm.CategoryId);
                await PopulateTags();
                return View(vm);
            }

            post.Title = vm.Title;
            post.Content = vm.Content;
            post.CategoryId = vm.CategoryId;

            post.Tags.Clear();
            var selectedTags = await _context.Tags
                .Where(t => vm.SelectedTagIds.Contains(t.Id))
                .ToListAsync();
            foreach (var tag in selectedTags)
                post.Tags.Add(tag);

            if (vm.ImageFile != null)
            {
                var newPath = await SaveImageAsync(vm.ImageFile);
                DeleteImage(post.ImagePath);
                post.ImagePath = newPath;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                DeleteImage(post.ImagePath);
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ---------- Helpers ----------

        private async Task PopulateCategories(int? selectedId = null)
        {
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
        }

        private void ValidateImage(IFormFile? file, string fieldName)
        {
            if (file == null || file.Length == 0) return;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                ModelState.AddModelError(fieldName, "Allowed formats: jpg, jpeg, png, webp");

            if (file.Length > MaxFileSize)
                ModelState.AddModelError(fieldName, "Image size must be less than 2 MB");
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var folder = Path.Combine(_environment.WebRootPath, "images");
            Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName).ToLowerInvariant();
            var fullPath = Path.Combine(folder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/images/" + fileName;
        }

        private void DeleteImage(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
        private async Task PopulateTags()
        {
            ViewBag.Tags = await _context.Tags.OrderBy(t => t.Name).ToListAsync();
        }
    }
}