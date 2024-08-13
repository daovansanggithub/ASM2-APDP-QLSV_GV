namespace WebApplication10.Models
{
    public class CourseDetailsViewModel
    {
        public string CourseName { get; set; } = string.Empty;
        public List<StudentDetailsViewModel> Students { get; set; } = new List<StudentDetailsViewModel>();
    }
}
