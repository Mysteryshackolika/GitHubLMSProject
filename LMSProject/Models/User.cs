using Microsoft.AspNetCore.Identity;

namespace LMSProject.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
        public string? Title { get; set; }
        public string? Company { get; set; }
        public string? Website { get; set; }
        public string? Location { get; set; }
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public ICollection<Course> TeacherCourses { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Certificate> Certificates { get; set; }
    }
}