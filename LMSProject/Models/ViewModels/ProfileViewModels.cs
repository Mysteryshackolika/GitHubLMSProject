namespace LMSProject.Models.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<string> Roles { get; set; }
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public int EnrolledCourses { get; set; }
        public List<Certificate> Certificates { get; set; }
        public List<ActivityViewModel> RecentActivity { get; set; }
    }
}