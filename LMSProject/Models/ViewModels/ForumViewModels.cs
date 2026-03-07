using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LMSProject.Models.ViewModels
{
    public class ForumIndexViewModel
    {
        public List<ForumCategory> Categories { get; set; }
        public List<ForumTopic> RecentTopics { get; set; }
        public List<ForumTopic> PopularTopics { get; set; }
        public List<ForumTopic> UnansweredTopics { get; set; }
        public int TotalTopics { get; set; }
        public int TotalReplies { get; set; }
        public int TotalUsers { get; set; }
    }

    public class ForumCategoryViewModel
    {
        public ForumCategory Category { get; set; }
        public List<ForumTopic> Topics { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalTopics { get; set; }
        public string SearchTerm { get; set; }
    }

    public class ForumTopicViewModel
    {
        public ForumTopic Topic { get; set; }
        public List<ForumReply> Replies { get; set; }
        public bool IsFollowing { get; set; }
        public int TotalReplies { get; set; }
        public int TotalViews { get; set; }
        public string ReplyContent { get; set; }
        public int? ParentReplyId { get; set; }
    }

    public class CreateTopicViewModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; }
        public bool UseEditor { get; set; } = true;
    }

    public class ForumSearchViewModel
    {
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public string SortBy { get; set; }
        public bool ShowSolved { get; set; } = true;
        public bool ShowUnanswered { get; set; }
        public List<ForumTopic> Results { get; set; }
        public List<ForumCategory> Categories { get; set; }
        public int TotalResults { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}