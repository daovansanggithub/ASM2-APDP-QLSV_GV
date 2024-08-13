using System.ComponentModel.DataAnnotations;

namespace WebApplication10.Models
{
    public class CourseAssignmentTeacherViewModel
    {
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [Display(Name = "Student Name")]
        public string StudentName { get; set; } = string.Empty;
    }
}
