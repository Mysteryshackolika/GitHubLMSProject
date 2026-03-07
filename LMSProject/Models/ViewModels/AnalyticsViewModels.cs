using System;
using System.Collections.Generic;

namespace LMSProject.Models.ViewModels
{
    public class AnalyticsDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalCourses { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalCompletions { get; set; }
        public double UserGrowth { get; set; }
        public double CourseGrowth { get; set; }
        public double EnrollmentGrowth { get; set; }
        public int ActiveUsersToday { get; set; }
        public int ActiveUsersThisWeek { get; set; }
        public int ActiveUsersThisMonth { get; set; }
        public List<ChartDataPoint> UserRegistrationChart { get; set; }
        public List<ChartDataPoint> EnrollmentChart { get; set; }
        public List<ChartDataPoint> RevenueChart { get; set; }
        public List<PopularCourseViewModel> PopularCourses { get; set; }
        public Dictionary<string, int> UserRoleDistribution { get; set; }
        public Dictionary<string, int> UserCountryDistribution { get; set; }
        public int TotalQuizzes { get; set; }
        public int TotalQuizAttempts { get; set; }
        public double AverageQuizScore { get; set; }
        public int TotalForumTopics { get; set; }
        public int TotalForumReplies { get; set; }
        public double AverageRepliesPerTopic { get; set; }
    }

    public class TeacherAnalyticsViewModel
    {
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalCompletions { get; set; }
        public double AverageCompletionRate { get; set; }
        public List<CourseAnalyticsViewModel> CourseAnalytics { get; set; }
        public List<ChartDataPoint> EnrollmentTrend { get; set; }
        public List<ChartDataPoint> RevenueTrend { get; set; }
        public Dictionary<string, int> StudentCountryDistribution { get; set; }
        public List<RecentActivityViewModel> RecentActivities { get; set; }
    }

    public class StudentAnalyticsViewModel
    {
        public int TotalEnrolledCourses { get; set; }
        public int CompletedCourses { get; set; }
        public double CompletionRate { get; set; }
        public int TotalPoints { get; set; }
        public int CurrentStreak { get; set; }
        public int TotalQuizzesTaken { get; set; }
        public double AverageQuizScore { get; set; }
        public int TotalForumPosts { get; set; }
        public List<ChartDataPoint> LearningActivity { get; set; }
        public List<ChartDataPoint> PointsHistory { get; set; }
        public List<CourseProgressViewModel> CourseProgress { get; set; }
        public List<Badge> RecentBadges { get; set; }
        public List<RecentActivityViewModel> RecentActivities { get; set; }
    }

    public class CourseAnalyticsViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalCompletions { get; set; }
        public double CompletionRate { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int TotalLessons { get; set; }
        public double AverageLessonProgress { get; set; }
        public List<ChartDataPoint> EnrollmentChart { get; set; }
        public List<StudentProgressViewModel> StudentProgress { get; set; }
        public List<LessonEngagementViewModel> LessonEngagement { get; set; }
    }

    public class ChartDataPoint
    {
        public string Label { get; set; }
        public double Value { get; set; }
        public string Color { get; set; }
    }

    public class PopularCourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string TeacherName { get; set; }
        public int EnrollmentCount { get; set; }
        public int CompletionCount { get; set; }
        public double CompletionRate { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }

    public class StudentProgressViewModel
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string ProfilePicture { get; set; }
        public double Progress { get; set; }
        public DateTime LastAccess { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public double QuizScore { get; set; }
    }

    public class CourseProgressViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public double Progress { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public DateTime EnrolledAt { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class LessonEngagementViewModel
    {
        public string LessonTitle { get; set; }
        public int TotalViews { get; set; }
        public int UniqueStudents { get; set; }
        public double AverageWatchTime { get; set; }
        public double CompletionRate { get; set; }
        public List<ChartDataPoint> ViewTrend { get; set; }
    }

    public class RecentActivityViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
    }

    public class TimeSeriesData
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public double Value { get; set; }
    }

    public class ExportAnalyticsViewModel
    {
        public string ReportType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Format { get; set; }
        public List<string> Columns { get; set; }
        public Dictionary<string, object> Filters { get; set; }
    }
}