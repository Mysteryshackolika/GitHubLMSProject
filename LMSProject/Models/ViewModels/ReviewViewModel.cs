namespace LMSProject.Models.ViewModels
{
    public class ReviewViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public List<Review> Reviews { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingCounts { get; set; }
        public Review UserReview { get; set; }
        public bool CanReview { get; set; }
    }
}