using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string TeacherId { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ThumbnailUrl { get; set; }
        public int? CategoryId { get; set; }

        [ForeignKey("TeacherId")]
        public User Teacher { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Quiz> Quizzes { get; set; }
    }
}