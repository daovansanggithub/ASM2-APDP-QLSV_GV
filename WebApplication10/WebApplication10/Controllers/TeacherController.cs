using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    public class TeacherController : Controller
    {
        private readonly QlsvMvc2Context _context;

        public TeacherController(QlsvMvc2Context context)
        {
            _context = context;
        }


        // test 
        public async Task<IActionResult> TeacherIndex()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");

            if (teacherId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherId == teacherId);

            if (teacher == null)
            {
                return RedirectToAction("Login", "User");
            }

            var courseAssignments = await _context.CourseAssignments
                .Where(ca => ca.TeacherId == teacherId)
                .Include(ca => ca.Course)
                .Include(ca => ca.Student)
                .Select(ca => new CourseAssignmentTeacherViewModel
                {
                    CourseName = ca.Course.CourseName,
                    StudentName = ca.Student.StudentName
                })
                .ToListAsync();

            var model = new TeacherViewModel
            {
                TeacherName = teacher.Name,
                CourseAssignmentViewModels = courseAssignments
            };

            return View(model);
        }


        // ===========================hiển thị khóa học của giáo viên đăng nhập vào hệ thống =======================
		public async Task<IActionResult> TeacherViewCourse()
		{
			var teacherId = HttpContext.Session.GetInt32("TeacherId");

			if (teacherId == null)
			{
				return RedirectToAction("Login", "User");
			}

			var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherId == teacherId);

			if (teacher == null)
			{
				return RedirectToAction("Login", "User");
			}

			var distinctCourses = await _context.CourseAssignments
				.Where(ca => ca.TeacherId == teacherId)
				.Include(ca => ca.Course)
				.Select(ca => ca.Course)
				.Distinct()
				.ToListAsync();

			var model = new ViewStudentTeacherViewModel
			{
				TeacherName = teacher.Name,
				Courses = distinctCourses.Select(course => new ViewCourseTeacherViewModel
				{
					CourseId = course.CourseId,
					CourseName = course.CourseName
				}).ToList()
			};

			return View(model);
		}


        // hiển thị chi tiết học sinh của khóa học đó mà giáo viên dạy
        public async Task<IActionResult> CourseDetails(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.CourseAssignments)
                    .ThenInclude(ca => ca.Student)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
            {
                return NotFound();
            }

            var students = course.CourseAssignments.Select(ca => ca.Student).ToList();

            var model = new CourseDetailsViewModel
            {
                CourseName = course.CourseName,
                Students = students.Select(s => new StudentDetailsViewModel
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName
                }).ToList()
            };

            return View(model);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
