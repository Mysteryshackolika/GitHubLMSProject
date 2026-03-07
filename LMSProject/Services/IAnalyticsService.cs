using LMSProject.Models.ViewModels;

namespace LMSProject.Services
{
    public interface IAnalyticsService
    {
        Task<AnalyticsDashboardViewModel> GetAdminAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<TeacherAnalyticsViewModel> GetTeacherAnalyticsAsync(string teacherId, DateTime? startDate = null, DateTime? endDate = null);
        Task<StudentAnalyticsViewModel> GetStudentAnalyticsAsync(string studentId);
        Task<CourseAnalyticsViewModel> GetCourseAnalyticsAsync(int courseId);
        Task<List<TimeSeriesData>> GetUserRegistrationTimeSeriesAsync(DateTime startDate, DateTime endDate, string interval = "day");
        Task<List<TimeSeriesData>> GetEnrollmentTimeSeriesAsync(DateTime startDate, DateTime endDate, int? courseId = null);
        Task<int> GetActiveUsersCountAsync(int minutes = 5);
        Task<Dictionary<string, int>> GetUserActivityHeatmapAsync();
    }
}