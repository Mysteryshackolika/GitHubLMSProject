namespace LMSProject.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalCourses { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalLessons { get; set; }
        public List<Course> RecentCourses { get; set; }
        public List<User> RecentUsers { get; set; }
        public List<CourseStatsViewModel> PopularCourses { get; set; }
    }

    public class CourseStatsViewModel
    {
        public string CourseTitle { get; set; }
        public int EnrollmentCount { get; set; }
    }

    public class TeacherDashboardViewModel
    {
        public List<Course> MyCourses { get; set; }
        public int TotalStudents => MyCourses?.Sum(c => c.Enrollments?.Count ?? 0) ?? 0;
        public int TotalLessons => MyCourses?.Sum(c => c.Lessons?.Count ?? 0) ?? 0;
        public int PendingTasks { get; set; }
        public List<Enrollment> RecentEnrollments { get; set; }
    }

    public class StudentDashboardViewModel
    {
        public List<Enrollment> MyEnrollments { get; set; }
        public int CompletedCourses { get; set; }
        public int InProgressCourses => MyEnrollments?.Count(e => !e.IsCompleted) ?? 0;
        public List<string> Certificates { get; set; }
        public List<ActivityViewModel> RecentActivity { get; set; }
    }

    public class ActivityViewModel
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
    }
}