using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    public class HomeController : Controller
    {

        private readonly QlsvMvc2Context _context;

        public HomeController(QlsvMvc2Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy danh sách khóa học cùng với giáo viên, loại bỏ các bản ghi trùng lặp
            var coursesWithTeachers = await _context.CourseAssignments
                .Include(ca => ca.Course)
                .Include(ca => ca.Teacher)
                .GroupBy(ca => new { ca.CourseId, ca.TeacherId })
                .Select(g => g.FirstOrDefault()) // Chỉ chọn một bản ghi cho mỗi nhóm
                .ToListAsync();

            // Truyền dữ liệu vào view
            return View(coursesWithTeachers);
        }


        // Action để hiển thị danh sách sinh viên của một khóa học
        public async Task<IActionResult> StudentsInCourse(int courseId)
        {
            // Lấy danh sách sinh viên thuộc khóa học
            var studentsInCourse = await _context.CourseAssignments
                .Where(ca => ca.CourseId == courseId)
                .Include(ca => ca.Student)
                .Select(ca => ca.Student)
                .ToListAsync();

            // Lấy thông tin khóa học để hiển thị
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
            {
                return NotFound();
            }

            // Truyền thông tin khóa học và danh sách sinh viên vào ViewBag
            ViewBag.CourseName = course.CourseName;
           

            return View(studentsInCourse);
        }

    }
}
