using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string TeacherId { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? ThumbnailUrl { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("TeacherId")]
        public User? Teacher { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        // Navigation properties - ƏLAVƏ ET
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}