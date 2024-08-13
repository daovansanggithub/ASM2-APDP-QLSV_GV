using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly QlsvMvc2Context _context;

        public AssignmentController(QlsvMvc2Context context)
        {
            _context = context;
        }

        // GET: CourseAssignment
        public async Task<IActionResult> Index()
        {
            var courseAssignments = await _context.CourseAssignments
                .Include(ca => ca.Teacher)
                .Include(ca => ca.Course)
                .Include(ca => ca.Student)
                .Select(ca => new CourseAssignmentViewModel
                {
                    CourseAssignmentId = ca.CourseAssignmentId,
                    TeacherName = ca.Teacher.Name,
                    CourseName = ca.Course.CourseName,
                    StudentName = ca.Student.StudentName
                })
                .ToListAsync();

            return View(courseAssignments);
        }

        // GET: CourseAssignment/Create
        public async Task<IActionResult> Create()
        {
            var model = new CourseAssignmentCreateViewModel
            {
                Teachers = await _context.Teachers
                    .Select(t => new SelectListItem
                    {
                        Value = t.TeacherId.ToString(),
                        Text = t.Name
                    }).ToListAsync(),

                Courses = await _context.Courses
                    .Select(c => new SelectListItem
                    {
                        Value = c.CourseId.ToString(),
                        Text = c.CourseName
                    }).ToListAsync(),

                Students = await _context.Students
                    .Select(s => new SelectListItem
                    {
                        Value = s.StudentId.ToString(),
                        Text = s.StudentName
                    }).ToListAsync()
            };

            return View(model);
        }
        // POST: CourseAssignment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseAssignmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var courseAssignment = new CourseAssignment
                {
                    TeacherId = model.TeacherId,
                    CourseId = model.CourseId,
                    StudentId = model.StudentId
                };

                _context.CourseAssignments.Add(courseAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Hoặc chuyển hướng đến trang khác
            }

            // Nếu có lỗi, hãy trả về view với dữ liệu đã nhập
            return View(model);
        }


        // delete
        // GET: CourseAssignment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignments
                .Include(ca => ca.Teacher)
                .Include(ca => ca.Course)
                .Include(ca => ca.Student)
                .Select(ca => new CourseAssignmentViewModel
                {
                    CourseAssignmentId = ca.CourseAssignmentId,
                    TeacherName = ca.Teacher.Name,
                    CourseName = ca.Course.CourseName,
                    StudentName = ca.Student.StudentName
                })
                .FirstOrDefaultAsync(ca => ca.CourseAssignmentId == id);

            if (courseAssignment == null)
            {
                return NotFound();
            }

            return View(courseAssignment);
        }

        // POST: CourseAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseAssignment = await _context.CourseAssignments.FindAsync(id);

            if (courseAssignment != null)
            {
                _context.CourseAssignments.Remove(courseAssignment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



    }
}
