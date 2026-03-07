using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class ForumCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public int? CourseId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        public ICollection<ForumTopic> Topics { get; set; }
    }

    public class ForumTopic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSolved { get; set; }
        public int Views { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastReplyAt { get; set; }
        public string? LastReplyUserId { get; set; }

        [ForeignKey("CategoryId")]
        public ForumCategory Category { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("LastReplyUserId")]
        public User LastReplyUser { get; set; }
        public ICollection<ForumReply> Replies { get; set; }
        public ICollection<ForumTopicView> TopicViews { get; set; }
        public ICollection<ForumTopicFollow> Followers { get; set; }
    }

    public class ForumReply
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public int TopicId { get; set; }
        public string UserId { get; set; }
        public int? ParentReplyId { get; set; }
        public bool IsSolution { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int LikeCount { get; set; }

        [ForeignKey("TopicId")]
        public ForumTopic Topic { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("ParentReplyId")]
        public ForumReply ParentReply { get; set; }
        public ICollection<ForumReply> ChildReplies { get; set; }
        public ICollection<ForumReplyLike> Likes { get; set; }
    }

    public class ForumTopicView
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string UserId { get; set; }
        public DateTime ViewedAt { get; set; } = DateTime.Now;
        public ForumTopic Topic { get; set; }
        public User User { get; set; }
    }

    public class ForumTopicFollow
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string UserId { get; set; }
        public DateTime FollowedAt { get; set; } = DateTime.Now;
        public ForumTopic Topic { get; set; }
        public User User { get; set; }
    }

    public class ForumReplyLike
    {
        public int Id { get; set; }
        public int ReplyId { get; set; }
        public string UserId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.Now;
        public ForumReply Reply { get; set; }
        public User User { get; set; }
    }
}