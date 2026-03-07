using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using LMSProject.Data;
using LMSProject.Hubs;
using LMSProject.Models;
using Microsoft.AspNetCore.Identity;

namespace LMSProject.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            ApplicationDbContext context,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SendNotificationAsync(
            string userId,
            string title,
            string message,
            NotificationType type,
            string? actionUrl = null,
            string? senderId = null,
            NotificationPriority priority = NotificationPriority.Normal)
        {
            var notification = new Notification
            {
                UserId = userId,
                SenderId = senderId,
                Title = title,
                Message = message,
                Type = type,
                Priority = priority,
                ActionUrl = actionUrl,
                Icon = GetIconForType(type),
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group($"user-{userId}")
                .SendAsync("ReceiveNotification", new
                {
                    notification.Id,
                    notification.Title,
                    notification.Message,
                    notification.Type,
                    notification.Priority,
                    notification.Icon,
                    notification.ActionUrl,
                    Time = notification.CreatedAt.ToString("HH:mm"),
                    IsRead = false
                });
        }

        public async Task SendNotificationToUsersAsync(
            List<string> userIds,
            string title,
            string message,
            NotificationType type,
            string? actionUrl = null)
        {
            foreach (var userId in userIds)
            {
                await SendNotificationAsync(userId, title, message, type, actionUrl);
            }
        }

        public async Task SendNotificationToRoleAsync(
     string role,
     string title,
     string message,
     NotificationType type,
     string? actionUrl = null)
        {
            // Əvvəlcə role ID-ni tap
            var roleEntity = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role);
            if (roleEntity == null) return;

            // Sonra həmin role aid user ID-ləri tap
            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == roleEntity.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            await SendNotificationToUsersAsync(userIds, title, message, type, actionUrl);
        }

        public async Task SendNotificationToCourseStudentsAsync(
            int courseId,
            string title,
            string message,
            NotificationType type,
            string? actionUrl = null)
        {
            var studentIds = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToListAsync();

            await SendNotificationToUsersAsync(studentIds, title, message, type, actionUrl);
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId, int page = 1, int pageSize = 20)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId, string userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
                await _context.SaveChangesAsync();

                await _hubContext.Clients.Group($"user-{userId}")
                    .SendAsync("NotificationRead", notificationId);
            }
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group($"user-{userId}")
                .SendAsync("AllNotificationsRead");
        }

        public async Task DeleteNotificationAsync(int notificationId, string userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        private string GetIconForType(NotificationType type)
        {
            return type switch
            {
                NotificationType.System => "bi-gear",
                NotificationType.Forum => "bi-chat-dots",
                NotificationType.Course => "bi-book",
                NotificationType.Quiz => "bi-puzzle",
                NotificationType.Certificate => "bi-award",
                NotificationType.Achievement => "bi-trophy",
                NotificationType.Message => "bi-envelope",
                NotificationType.Reminder => "bi-bell",
                NotificationType.Announcement => "bi-megaphone",
                _ => "bi-bell"
            };
        }
    }
}