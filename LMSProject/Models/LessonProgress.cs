using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class LessonProgress
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int LessonId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TimeSpan? WatchDuration { get; set; }
        public DateTime LastAccessedAt { get; set; } = DateTime.Now;

        [ForeignKey("StudentId")]
        public User Student { get; set; }
        [ForeignKey("LessonId")]
        public Lesson Lesson { get; set; }
    }
}