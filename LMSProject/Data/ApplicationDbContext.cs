using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMSProject.Models;

namespace LMSProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewHelpful> ReviewHelpfuls { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<ForumReply> ForumReplies { get; set; }
        public DbSet<ForumTopicView> ForumTopicViews { get; set; }
        public DbSet<ForumTopicFollow> ForumTopicFollows { get; set; }
        public DbSet<ForumReplyLike> ForumReplyLikes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationPreference> NotificationPreferences { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<UserPoints> UserPoints { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }
        public DbSet<UserStreak> UserStreaks { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<LessonProgress> LessonProgresses { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ========== DECIMAL PRECISION KONFİQURASİYASI ==========
            // Bütün decimal property-lər üçün precision və scale təyin et
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                    {
                        property.SetPrecision(18);
                        property.SetScale(2);
                    }
                }
            }

            // ========== ƏLAQƏLƏR ==========
            builder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(u => u.TeacherCourses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== UNİKAL INDEKSLƏR ==========
            builder.Entity<Review>()
                .HasIndex(r => new { r.CourseId, r.UserId })
                .IsUnique();

            builder.Entity<ReviewHelpful>()
                .HasIndex(rh => new { rh.ReviewId, rh.UserId })
                .IsUnique();

            builder.Entity<Certificate>()
                .HasIndex(c => c.CertificateNumber)
                .IsUnique();

            builder.Entity<Certificate>()
                .HasIndex(c => new { c.StudentId, c.CourseId })
                .IsUnique();

            builder.Entity<LessonProgress>()
                .HasIndex(lp => new { lp.StudentId, lp.LessonId })
                .IsUnique();

            // ========== QUIZ MODELLƏRİ ÜÇƏN PRECISION ==========
            builder.Entity<QuizAttempt>()
                .Property(q => q.PercentageScore)
                .HasPrecision(5, 2);
        }
    }
}