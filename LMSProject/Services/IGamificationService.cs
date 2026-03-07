using LMSProject.Models;
using LMSProject.Models.ViewModels;

namespace LMSProject.Services
{
    public interface IGamificationService
    {
        Task AddPointsAsync(string userId, int points, PointTransactionType type, string description, string referenceId = null);
        Task CheckAndAwardBadgesAsync(string userId);
        Task ProcessDailyLoginAsync(string userId);
        Task ProcessCourseCompletionAsync(string userId, int courseId);
        Task ProcessQuizCompletionAsync(string userId, int quizId, int score, bool passed);
        Task ProcessForumActivityAsync(string userId, string action);
        Task<int> CalculateUserLevelAsync(int totalPoints);
        Task<UserPoints> GetUserPointsAsync(string userId);
        Task<List<UserBadge>> GetUserBadgesAsync(string userId);
        Task<List<BadgeProgressViewModel>> GetBadgeProgressAsync(string userId);
    }
}