using System;
using System.Collections.Generic;

namespace LMSProject.Models.ViewModels
{
    public class UserProfileGamificationViewModel
    {
        public UserPoints Points { get; set; }
        public List<UserBadge> Badges { get; set; }
        public int NextLevelPoints { get; set; }
        public int ProgressPercentage { get; set; }
        public List<PointTransaction> RecentTransactions { get; set; }
        public UserStreak Streak { get; set; }
    }

    public class BadgeProgressViewModel
    {
        public Badge Badge { get; set; }
        public int CurrentProgress { get; set; }
        public int RequiredProgress { get; set; }
        public double Percentage { get; set; }
        public bool IsEarned { get; set; }
    }

    public class LeaderboardViewModel
    {
        public LeaderboardType Type { get; set; }
        public List<LeaderboardEntryViewModel> Entries { get; set; }
        public LeaderboardEntryViewModel CurrentUserEntry { get; set; }
        public DateTime Period { get; set; }
    }

    public class LeaderboardEntryViewModel
    {
        public int Rank { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int Points { get; set; }
        public int Level { get; set; }
        public List<Badge> RecentBadges { get; set; }
        public bool IsCurrentUser { get; set; }
    }

    public class GamificationStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalBadgesEarned { get; set; }
        public int TotalPointsEarned { get; set; }
        public int ActiveUsersToday { get; set; }
        public List<Badge> MostEarnedBadges { get; set; }
        public Dictionary<string, int> PointsByType { get; set; }
    }
}