using System.ComponentModel.DataAnnotations;

namespace WebApplication10.Models
{
    public class CourseAssignmentViewModel
    {
        public int CourseAssignmentId { get; set; }

        [Display(Name = "Teacher Name")]
        public string TeacherName { get; set; } = string.Empty;

        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [Display(Name = "Student Name")]
        public string StudentName { get; set; } = string.Empty;
    }
}
