using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication10.Models
{
    public class CourseAssignmentCreateViewModel
    {
        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }

        // Optional: Add lists of teachers, courses, and students for dropdowns
        public IEnumerable<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Courses { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Students { get; set; } = new List<SelectListItem>();

    }
}
