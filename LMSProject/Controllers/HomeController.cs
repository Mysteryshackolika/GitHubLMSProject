using Microsoft.AspNetCore.Mvc;
using LMSProject.Data;
using Microsoft.EntityFrameworkCore;

namespace LMSProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Lessons)
                .OrderByDescending(c => c.CreatedAt)
                .Take(6)
                .ToListAsync();

            return View(courses);
        }
    }
}