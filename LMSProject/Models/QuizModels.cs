using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSProject.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int CourseId { get; set; }
        public int? LessonId { get; set; }
        [Range(1, 180)]
        public int TimeLimit { get; set; } = 30;
        [Range(1, 100)]
        public int PassingScore { get; set; } = 70;
        [Range(1, 10)]
        public int MaxAttempts { get; set; } = 3;
        public bool ShuffleQuestions { get; set; } = false;
        public bool ShowResults { get; set; } = true;
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public int TotalPoints { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [ForeignKey("LessonId")]
        public Lesson Lesson { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<QuizAttempt> Attempts { get; set; }
    }

    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QuizId { get; set; }
        [Required]
        public string Text { get; set; }
        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
        public int Points { get; set; } = 1;
        public string? Explanation { get; set; }
        public int Order { get; set; }
        public string? ImageUrl { get; set; }

        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
    }

    public class Answer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int Order { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
    }

    public class QuizAttempt
    {
        public int Id { get; set; }
        [Required]
        public int QuizId { get; set; }
        [Required]
        public string StudentId { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
        public int Score { get; set; }
        public int TotalPoints { get; set; }
        public double PercentageScore { get; set; }
        public bool IsPassed { get; set; }
        public int AttemptNumber { get; set; }
        public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.InProgress;

        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }
        [ForeignKey("StudentId")]
        public User Student { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
    }

    public class UserAnswer
    {
        public int Id { get; set; }
        public int QuizAttemptId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string? TextAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }

        [ForeignKey("QuizAttemptId")]
        public QuizAttempt QuizAttempt { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        [ForeignKey("AnswerId")]
        public Answer SelectedAnswer { get; set; }
    }

    public enum QuestionType
    {
        MultipleChoice = 1,
        MultipleAnswer = 2,
        TrueFalse = 3,
        FillInBlank = 4,
        Essay = 5
    }

    public enum QuizAttemptStatus
    {
        InProgress = 1,
        Completed = 2,
        TimedOut = 3,
        Abandoned = 4
    }
}