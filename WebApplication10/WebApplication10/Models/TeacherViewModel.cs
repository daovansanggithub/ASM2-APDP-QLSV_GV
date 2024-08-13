namespace WebApplication10.Models
{
    public class TeacherViewModel
    {
        public string TeacherName { get; set; } = string.Empty;
        public List<CourseAssignmentTeacherViewModel> CourseAssignmentViewModels { get; set; } = new List<CourseAssignmentTeacherViewModel>();
    }
}

