namespace WebApplication10.Models
{
	public class ViewStudentTeacherViewModel
	{
		public string TeacherName { get; set; } = string.Empty;
		public List<ViewCourseTeacherViewModel> Courses { get; set; } = new List<ViewCourseTeacherViewModel>();
	}
}
