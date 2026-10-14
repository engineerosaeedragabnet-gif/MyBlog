using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.ViewModels;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel
            {
                LatestPosts = await _context.Posts
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Include(p => p.Tags)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(3)
                    .ToListAsync(),

                Categories = await _context.Categories
                    .AsNoTracking()
                    .OrderBy(c => c.Name)
                    .Select(c => new CategoryWithCountViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        PostsCount = c.Posts.Count
                    })
                    .ToListAsync()
            };

            return View(vm);
        }

        [Route("Home/NotFound")]
        public IActionResult NotFound404()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}