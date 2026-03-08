using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSProject.Data;
using LMSProject.Models;
using LMSProject.Models.ViewModels;

namespace LMSProject.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CoursesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Category)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Category)
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            var isEnrolled = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                isEnrolled = await _context.Enrollments
                    .AnyAsync(e => e.CourseId == id && e.StudentId == user.Id);
            }

            ViewBag.IsEnrolled = isEnrolled;

            return View(course);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                course.TeacherId = user.Id;
                course.CreatedAt = DateTime.Now;

                _context.Add(course);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Kurs uğurla yaradıldı!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == id && e.StudentId == user.Id);

            if (existingEnrollment == null)
            {
                var enrollment = new Enrollment
                {
                    CourseId = id,
                    StudentId = user.Id,
                    EnrolledAt = DateTime.Now,
                    IsCompleted = false
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Kursa uğurla qoşuldunuz!";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Search(CourseSearchViewModel model)
        {
            var query = _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                query = query.Where(c =>
                    c.Title.Contains(model.SearchTerm) ||
                    c.Description.Contains(model.SearchTerm));
            }

            if (model.CategoryId.HasValue && model.CategoryId.Value > 0)
            {
                query = query.Where(c => c.CategoryId == model.CategoryId);
            }

            var totalItems = await query.CountAsync();
            model.TotalPages = (int)Math.Ceiling(totalItems / (double)model.PageSize);

            model.Courses = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync();

            model.Categories = await _context.Categories.ToListAsync();

            return View(model);
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}