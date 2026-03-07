using System.ComponentModel.DataAnnotations;

namespace LMSProject.Models.ViewModels
{
    public class QuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int TimeLimit { get; set; }
        public int PassingScore { get; set; }
        public int MaxAttempts { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalPoints { get; set; }
        public int AttemptsUsed { get; set; }
        public QuizAttempt LastAttempt { get; set; }
        public bool CanTakeQuiz { get; set; }
        public string Message { get; set; }
    }

    public class QuizQuestionViewModel
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int QuestionIndex { get; set; }
        public int TotalQuestions { get; set; }
        public Question Question { get; set; }
        public int TimeRemaining { get; set; }
        public int AttemptId { get; set; }
    }

    public class QuizResultViewModel
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int Score { get; set; }
        public int TotalPoints { get; set; }
        public double Percentage { get; set; }
        public bool IsPassed { get; set; }
        public int PassingScore { get; set; }
        public DateTime CompletedAt { get; set; }
        public int AttemptNumber { get; set; }
        public List<QuestionResultViewModel> QuestionResults { get; set; }
    }

    public class QuestionResultViewModel
    {
        public string QuestionText { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
        public int PointsPossible { get; set; }
        public string Explanation { get; set; }
    }

    public class CreateQuizViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int CourseId { get; set; }

        public int? LessonId { get; set; }

        [Required]
        [Range(1, 180)]
        public int TimeLimit { get; set; } = 30;

        [Required]
        [Range(1, 100)]
        public int PassingScore { get; set; } = 70;

        [Required]
        [Range(1, 10)]
        public int MaxAttempts { get; set; } = 3;

        public bool ShuffleQuestions { get; set; }
        public bool ShowResults { get; set; } = true;

        public List<Course> AvailableCourses { get; set; }
        public List<Lesson> AvailableLessons { get; set; }
    }
}