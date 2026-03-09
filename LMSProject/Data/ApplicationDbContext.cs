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

            // ========== BÜTUN FOREIGN KEY LƏR ÜÇÜN RESTRICT ==========
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // ========== ƏLAQƏLƏR ==========
            builder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.StudentId);

            builder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)  // Enrollments property-si Course-da var
                .HasForeignKey(e => e.CourseId);

            builder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(u => u.TeacherCourses)
                .HasForeignKey(c => c.TeacherId);

            builder.Entity<Lesson>()
                .HasOne(l => l.Course)
                .WithMany(c => c.Lessons)  // Lessons property-si Course-da var
                .HasForeignKey(l => l.CourseId);

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
        }
    }
}