using LMSProject.Models;

namespace LMSProject.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(
            string userId,
            string title,
            string message,
            NotificationType type,
            string? actionUrl = null,
            string? senderId = null,
            NotificationPriority priority = NotificationPriority.Normal);

        Task SendNotificationToUsersAsync(
            List<string> userIds,
            string title,
            string message,
            NotificationType type,
            string? actionUrl = null);

        Task SendNotificationToRoleAsync(
            string role,
            string title,
            string message,
            NotificationType type,
            string? actionUrl = null);

        Task SendNotificationToCourseStudentsAsync(
            int courseId,
            string title,
            string message,
            NotificationType type,
            string? actionUrl = null);

        Task<int> GetUnreadCountAsync(string userId);

        Task<List<Notification>> GetUserNotificationsAsync(string userId, int page = 1, int pageSize = 20);

        Task MarkAsReadAsync(int notificationId, string userId);

        Task MarkAllAsReadAsync(string userId);

        Task DeleteNotificationAsync(int notificationId, string userId);
    }
}