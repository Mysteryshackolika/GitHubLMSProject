using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.CategoryList = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            ModelState.Remove("Teacher");
            ModelState.Remove("Category");

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

            ViewBag.CategoryList = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

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
    }
}