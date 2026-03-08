using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSProject.Data;
using LMSProject.Models;
using LMSProject.Models.ViewModels;

namespace LMSProject.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                return RedirectToAction("AdminDashboard");
            }
            else if (roles.Contains("Teacher"))
            {
                return RedirectToAction("TeacherDashboard");
            }
            else
            {
                return RedirectToAction("StudentDashboard");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalCourses = await _context.Courses.CountAsync(),
                TotalEnrollments = await _context.Enrollments.CountAsync(),
                TotalLessons = await _context.Lessons.CountAsync(),
                RecentCourses = await _context.Courses
                    .Include(c => c.Teacher)
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                RecentUsers = await _context.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                PopularCourses = new List<CourseStatsViewModel>()
            };

            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> TeacherDashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new TeacherDashboardViewModel
            {
                MyCourses = await _context.Courses
                    .Where(c => c.TeacherId == user.Id)
                    .Include(c => c.Lessons)
                    .Include(c => c.Enrollments)
                    .ToListAsync(),
                PendingTasks = 0,
                RecentEnrollments = new List<Enrollment>()
            };

            return View(model);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentDashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new StudentDashboardViewModel
            {
                MyEnrollments = await _context.Enrollments
                    .Include(e => e.Course)
                        .ThenInclude(c => c.Teacher)
                    .Where(e => e.StudentId == user.Id)
                    .ToListAsync(),
                CompletedCourses = await _context.Enrollments
                    .CountAsync(e => e.StudentId == user.Id && e.IsCompleted),
                Certificates = new List<string>(),
                RecentActivity = new List<ActivityViewModel>()
            };

            return View(model);
        }
    }
}