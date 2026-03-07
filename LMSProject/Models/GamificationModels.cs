using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class Badge
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public BadgeType Type { get; set; }
        public BadgeRarity Rarity { get; set; } = BadgeRarity.Common;
        public int PointsReward { get; set; }
        public string Criteria { get; set; }
        public int? RequiredCount { get; set; }
        public bool IsHidden { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<UserBadge> UserBadges { get; set; }
    }

    public class UserBadge
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int BadgeId { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.Now;
        public bool IsDisplayed { get; set; } = true;

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("BadgeId")]
        public Badge Badge { get; set; }
    }

    public class UserPoints
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public int TotalPoints { get; set; }
        public int Level { get; set; } = 1;
        public int PointsToNextLevel { get; set; }
        public int CoursePoints { get; set; }
        public int QuizPoints { get; set; }
        public int ForumPoints { get; set; }
        public int StreakPoints { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateTime LastActivityDate { get; set; }
        public int Rank { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<PointTransaction> PointTransactions { get; set; }
    }

    public class PointTransaction
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public int Points { get; set; }
        public PointTransactionType Type { get; set; }
        public string Description { get; set; }
        public string ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public User User { get; set; }
    }

    public class UserStreak
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime? StreakStartDate { get; set; }
        public DateTime? StreakEndDate { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }

    public class Leaderboard
    {
        [Key]
        public int Id { get; set; }
        public LeaderboardType Type { get; set; }
        public DateTime Period { get; set; }
        public int Rank { get; set; }
        public string UserId { get; set; }
        public int Points { get; set; }
        public DateTime CalculatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public User User { get; set; }
    }

    public enum BadgeType
    {
        Course = 1,
        Quiz = 2,
        Forum = 3,
        Streak = 4,
        Achievement = 5,
        Social = 6,
        Milestone = 7
    }

    public enum BadgeRarity
    {
        Common = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4
    }

    public enum PointTransactionType
    {
        CourseCompleted = 1,
        QuizPassed = 2,
        ForumTopicCreated = 3,
        ForumReplyCreated = 4,
        DailyLogin = 5,
        StreakBonus = 6,
        BadgeEarned = 7,
        CourseReviewed = 8,
        ProfileCompleted = 9,
        Referral = 10,
        AdminAdjustment = 99
    }

    public enum LeaderboardType
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        AllTime = 4,
        Course = 5,
        Quiz = 6
    }
}