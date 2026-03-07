using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public string? SenderId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
        public string? ActionUrl { get; set; }
        public string? Icon { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ExpiresAt { get; set; }
        public bool EmailSent { get; set; } = false;
        public DateTime? EmailSentAt { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("SenderId")]
        public User Sender { get; set; }
    }

    public enum NotificationType
    {
        System = 1,
        Forum = 2,
        Course = 3,
        Quiz = 4,
        Certificate = 5,
        Achievement = 6,
        Message = 7,
        Reminder = 8,
        Announcement = 9
    }

    public enum NotificationPriority
    {
        Low = 1,
        Normal = 2,
        High = 3,
        Urgent = 4
    }

    public class NotificationPreference
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool AllowForumNotifications { get; set; } = true;
        public bool AllowCourseNotifications { get; set; } = true;
        public bool AllowQuizNotifications { get; set; } = true;
        public bool AllowCertificateNotifications { get; set; } = true;
        public bool AllowAchievementNotifications { get; set; } = true;
        public bool AllowMessageNotifications { get; set; } = true;
        public bool AllowReminderNotifications { get; set; } = true;
        public bool AllowAnnouncementNotifications { get; set; } = true;
        public bool EmailNotifications { get; set; } = true;
        public bool EmailForumReplies { get; set; } = true;
        public bool EmailQuizResults { get; set; } = true;
        public bool EmailCertificateAwarded { get; set; } = true;
        public bool PushNotifications { get; set; } = true;

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}