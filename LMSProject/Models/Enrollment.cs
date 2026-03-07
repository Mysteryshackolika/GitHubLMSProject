using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; }

        [ForeignKey("StudentId")]
        public User Student { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}