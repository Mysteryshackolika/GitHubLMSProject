using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class Certificate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CertificateNumber { get; set; }
        [Required]
        public string StudentId { get; set; }
        [Required]
        public int CourseId { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime? ExpiryDate { get; set; }
        public string CertificateUrl { get; set; }
        public bool IsVerified { get; set; } = true;

        [ForeignKey("StudentId")]
        public User Student { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}