using Microsoft.EntityFrameworkCore;
using LMSProject.Data;
using LMSProject.Models;
using LMSProject.Models.ViewModels;

namespace LMSProject.Services
{
    public class GamificationService : IGamificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public GamificationService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task AddPointsAsync(string userId, int points, PointTransactionType type, string description, string referenceId = null)
        {
            var userPoints = await GetOrCreateUserPointsAsync(userId);

            var transaction = new PointTransaction
            {
                UserId = userId,
                Points = points,
                Type = type,
                Description = description,
                ReferenceId = referenceId,
                CreatedAt = DateTime.Now
            };

            userPoints.TotalPoints += points;

            switch (type)
            {
                case PointTransactionType.CourseCompleted:
                    userPoints.CoursePoints += points;
                    break;
                case PointTransactionType.QuizPassed:
                    userPoints.QuizPoints += points;
                    break;
                case PointTransactionType.ForumTopicCreated:
                case PointTransactionType.ForumReplyCreated:
                    userPoints.ForumPoints += points;
                    break;
                case PointTransactionType.DailyLogin:
                case PointTransactionType.StreakBonus:
                    userPoints.StreakPoints += points;
                    break;
            }

            var newLevel = await CalculateUserLevelAsync(userPoints.TotalPoints);
            if (newLevel > userPoints.Level)
            {
                userPoints.Level = newLevel;

                await _notificationService.SendNotificationAsync(
                    userId,
                    "Səviyyə atladınız! 🎉",
                    $"Təbriklər! {newLevel}. səviyyəyə çatdınız.",
                    NotificationType.Achievement,
                    "/gamification");
            }

            userPoints.PointsToNextLevel = await GetPointsToNextLevelAsync(userPoints.TotalPoints);
            userPoints.LastActivityDate = DateTime.Now;

            _context.PointTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            await CheckAndAwardBadgesAsync(userId);
        }

        public async Task CheckAndAwardBadgesAsync(string userId)
        {
            var allBadges = await _context.Badges.ToListAsync();
            var userBadges = await _context.UserBadges
                .Where(ub => ub.UserId == userId)
                .Select(ub => ub.BadgeId)
                .ToListAsync();

            var userPoints = await GetUserPointsAsync(userId);
            var userEnrollments = await _context.Enrollments
                .Where(e => e.StudentId == userId)
                .ToListAsync();
            var userQuizzes = await _context.QuizAttempts
                .Where(qa => qa.StudentId == userId && qa.IsPassed)
                .ToListAsync();
            var userForumTopics = await _context.ForumTopics
                .CountAsync(t => t.UserId == userId);
            var userForumReplies = await _context.ForumReplies
                .CountAsync(r => r.UserId == userId);
            var userStreak = await _context.UserStreaks
                .FirstOrDefaultAsync(s => s.UserId == userId);

            foreach (var badge in allBadges)
            {
                if (userBadges.Contains(badge.Id)) continue;

                bool shouldAward = false;

                switch (badge.Type)
                {
                    case BadgeType.Course:
                        if (badge.Criteria == "first_course" && userEnrollments.Any(e => e.IsCompleted))
                            shouldAward = true;
                        else if (badge.Criteria == "five_courses" && userEnrollments.Count(e => e.IsCompleted) >= 5)
                            shouldAward = true;
                        else if (badge.Criteria == "ten_courses" && userEnrollments.Count(e => e.IsCompleted) >= 10)
                            shouldAward = true;
                        break;

                    case BadgeType.Quiz:
                        if (badge.Criteria == "first_quiz" && userQuizzes.Any())
                            shouldAward = true;
                        else if (badge.Criteria == "perfect_score" && userQuizzes.Any(q => q.PercentageScore == 100))
                            shouldAward = true;
                        else if (badge.Criteria == "quiz_master" && userQuizzes.Count >= 10)
                            shouldAward = true;
                        break;

                    case BadgeType.Forum:
                        if (badge.Criteria == "first_post" && (userForumTopics > 0 || userForumReplies > 0))
                            shouldAward = true;
                        else if (badge.Criteria == "helpful" && userForumReplies >= 10)
                            shouldAward = true;
                        else if (badge.Criteria == "expert" && userForumTopics >= 20)
                            shouldAward = true;
                        break;

                    case BadgeType.Streak:
                        if (badge.Criteria == "seven_day_streak" && userStreak?.CurrentStreak >= 7)
                            shouldAward = true;
                        else if (badge.Criteria == "thirty_day_streak" && userStreak?.CurrentStreak >= 30)
                            shouldAward = true;
                        else if (badge.Criteria == "hundred_day_streak" && userStreak?.CurrentStreak >= 100)
                            shouldAward = true;
                        break;

                    case BadgeType.Milestone:
                        if (badge.Criteria == "points_1000" && userPoints.TotalPoints >= 1000)
                            shouldAward = true;
                        else if (badge.Criteria == "points_5000" && userPoints.TotalPoints >= 5000)
                            shouldAward = true;
                        else if (badge.Criteria == "points_10000" && userPoints.TotalPoints >= 10000)
                            shouldAward = true;
                        else if (badge.Criteria == "level_5" && userPoints.Level >= 5)
                            shouldAward = true;
                        else if (badge.Criteria == "level_10" && userPoints.Level >= 10)
                            shouldAward = true;
                        break;
                }

                if (shouldAward)
                {
                    await AwardBadgeAsync(userId, badge.Id);
                }
            }
        }

        private async Task AwardBadgeAsync(string userId, int badgeId)
        {
            var userBadge = new UserBadge
            {
                UserId = userId,
                BadgeId = badgeId,
                EarnedAt = DateTime.Now
            };

            _context.UserBadges.Add(userBadge);

            var badge = await _context.Badges.FindAsync(badgeId);

            if (badge.PointsReward > 0)
            {
                await AddPointsAsync(userId, badge.PointsReward, PointTransactionType.BadgeEarned,
                    $"{badge.Name} badge-i qazandınız!");
            }

            await _context.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(
                userId,
                "Yeni Badge Qazandınız! 🏆",
                $"{badge.Name}: {badge.Description}",
                NotificationType.Achievement,
                "/gamification");
        }

        public async Task ProcessDailyLoginAsync(string userId)
        {
            var today = DateTime.Today;
            var streak = await _context.UserStreaks
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (streak == null)
            {
                streak = new UserStreak
                {
                    UserId = userId,
                    CurrentStreak = 1,
                    LongestStreak = 1,
                    LastLoginDate = today,
                    StreakStartDate = today
                };
                _context.UserStreaks.Add(streak);

                await AddPointsAsync(userId, 10, PointTransactionType.DailyLogin, "Gündəlik giriş");
            }
            else
            {
                var lastLogin = streak.LastLoginDate.Date;

                if (lastLogin == today)
                {
                    return;
                }

                if (lastLogin == today.AddDays(-1))
                {
                    streak.CurrentStreak++;
                    streak.LastLoginDate = today;

                    var bonusPoints = streak.CurrentStreak * 5;
                    await AddPointsAsync(userId, bonusPoints, PointTransactionType.StreakBonus,
                        $"{streak.CurrentStreak} günlük streak bonusu");
                }
                else
                {
                    streak.CurrentStreak = 1;
                    streak.StreakStartDate = today;
                    streak.LastLoginDate = today;

                    await AddPointsAsync(userId, 10, PointTransactionType.DailyLogin, "Gündəlik giriş");
                }

                if (streak.CurrentStreak > streak.LongestStreak)
                {
                    streak.LongestStreak = streak.CurrentStreak;
                }
            }

            await _context.SaveChangesAsync();

            var userPoints = await GetOrCreateUserPointsAsync(userId);
            userPoints.CurrentStreak = streak.CurrentStreak;
            userPoints.LongestStreak = streak.LongestStreak;
            userPoints.LastActivityDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task ProcessCourseCompletionAsync(string userId, int courseId)
        {
            await AddPointsAsync(userId, 100, PointTransactionType.CourseCompleted,
                "Kurs tamamlandı", courseId.ToString());
            await CheckAndAwardBadgesAsync(userId);
        }

        public async Task ProcessQuizCompletionAsync(string userId, int quizId, int score, bool passed)
        {
            if (passed)
            {
                var points = score switch
                {
                    >= 90 => 50,
                    >= 70 => 30,
                    _ => 20
                };

                await AddPointsAsync(userId, points, PointTransactionType.QuizPassed,
                    $"Quiz tamamlandı: {score}%", quizId.ToString());

                if (score == 100)
                {
                    await AddPointsAsync(userId, 30, PointTransactionType.QuizPassed,
                        "Mükəmməl bal! +30 bonus", quizId.ToString());
                }

                await CheckAndAwardBadgesAsync(userId);
            }
        }

        public async Task ProcessForumActivityAsync(string userId, string action)
        {
            var points = action switch
            {
                "topic" => 15,
                "reply" => 10,
                "solution" => 25,
                _ => 5
            };

            var description = action switch
            {
                "topic" => "Yeni forum mövzusu açdınız",
                "reply" => "Cavab yazdınız",
                "solution" => "Cavabınız həll olaraq işarələndi",
                _ => "Forum aktivliyi"
            };

            await AddPointsAsync(userId, points, PointTransactionType.ForumReplyCreated, description);
            await CheckAndAwardBadgesAsync(userId);
        }

        public async Task<int> CalculateUserLevelAsync(int totalPoints)
        {
            return (totalPoints / 500) + 1;
        }

        private async Task<int> GetPointsToNextLevelAsync(int totalPoints)
        {
            var currentLevel = await CalculateUserLevelAsync(totalPoints);
            var nextLevelPoints = currentLevel * 500;
            return nextLevelPoints - totalPoints;
        }

        public async Task<UserPoints> GetUserPointsAsync(string userId)
        {
            return await GetOrCreateUserPointsAsync(userId);
        }

        private async Task<UserPoints> GetOrCreateUserPointsAsync(string userId)
        {
            var userPoints = await _context.UserPoints
                .FirstOrDefaultAsync(up => up.UserId == userId);

            if (userPoints == null)
            {
                userPoints = new UserPoints
                {
                    UserId = userId,
                    TotalPoints = 0,
                    Level = 1,
                    PointsToNextLevel = 500,
                    LastActivityDate = DateTime.Now
                };
                _context.UserPoints.Add(userPoints);
                await _context.SaveChangesAsync();
            }

            return userPoints;
        }

        public async Task<List<UserBadge>> GetUserBadgesAsync(string userId)
        {
            return await _context.UserBadges
                .Include(ub => ub.Badge)
                .Where(ub => ub.UserId == userId)
                .OrderByDescending(ub => ub.EarnedAt)
                .ToListAsync();
        }

        public async Task<List<BadgeProgressViewModel>> GetBadgeProgressAsync(string userId)
        {
            var allBadges = await _context.Badges.ToListAsync();
            var userBadges = await _context.UserBadges
                .Where(ub => ub.UserId == userId)
                .Select(ub => ub.BadgeId)
                .ToListAsync();

            var progress = new List<BadgeProgressViewModel>();

            foreach (var badge in allBadges)
            {
                if (userBadges.Contains(badge.Id))
                {
                    progress.Add(new BadgeProgressViewModel
                    {
                        Badge = badge,
                        CurrentProgress = 100,
                        RequiredProgress = 100,
                        Percentage = 100,
                        IsEarned = true
                    });
                }
                else
                {
                    var currentProgress = await CalculateBadgeProgressAsync(userId, badge);
                    progress.Add(new BadgeProgressViewModel
                    {
                        Badge = badge,
                        CurrentProgress = currentProgress,
                        RequiredProgress = badge.RequiredCount ?? 1,
                        Percentage = (double)currentProgress / (badge.RequiredCount ?? 1) * 100,
                        IsEarned = false
                    });
                }
            }

            return progress;
        }

        private async Task<int> CalculateBadgeProgressAsync(string userId, Badge badge)
        {
            return badge.Type switch
            {
                BadgeType.Course => await _context.Enrollments
                    .CountAsync(e => e.StudentId == userId && e.IsCompleted),

                BadgeType.Quiz => await _context.QuizAttempts
                    .CountAsync(q => q.StudentId == userId && q.IsPassed),

                BadgeType.Forum => await _context.ForumTopics
                    .CountAsync(t => t.UserId == userId) +
                    await _context.ForumReplies.CountAsync(r => r.UserId == userId),

                BadgeType.Streak => (await _context.UserStreaks
                    .FirstOrDefaultAsync(s => s.UserId == userId))?.CurrentStreak ?? 0,

                BadgeType.Milestone => badge.Criteria switch
                {
                    "points_1000" => (await GetUserPointsAsync(userId)).TotalPoints,
                    "points_5000" => (await GetUserPointsAsync(userId)).TotalPoints,
                    "points_10000" => (await GetUserPointsAsync(userId)).TotalPoints,
                    "level_5" => (await GetUserPointsAsync(userId)).Level,
                    "level_10" => (await GetUserPointsAsync(userId)).Level,
                    _ => 0
                },

                _ => 0
            };
        }
    }
}