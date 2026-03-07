using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        [StringLength(1000)]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsApproved { get; set; } = true;
        public bool IsVerifiedPurchase { get; set; }
        public int HelpfulCount { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<ReviewHelpful> HelpfulVotes { get; set; }
    }

    public class ReviewHelpful
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public DateTime VotedAt { get; set; } = DateTime.Now;
        public Review Review { get; set; }
        public User User { get; set; }
    }
}