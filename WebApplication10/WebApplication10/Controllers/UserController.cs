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
    public class UserController : Controller
    {
        private readonly QlsvMvc2Context _context;

        public UserController(QlsvMvc2Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Get login form
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // Không kiểm tra mật khẩu, chỉ lưu thông tin vào session
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserRole", user.Role);

                    if (user.Role == "teacher")
                    {
                        var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.UserId);
                        if (teacher != null)
                        {
                            HttpContext.Session.SetInt32("TeacherId", teacher.TeacherId);
                            return RedirectToAction("TeacherViewCourse", "Teacher");
                        }
                    }
                    else if (user.Role == "admin")
                    {
                        return RedirectToAction("Index", "CRUD_User");
                    }
                    else if (user.Role == "student")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.err = "Tài khoản không tồn tại.";
                }
            }

            return View(model);
        }


        // Action method để xử lý đăng xuất
        public IActionResult Logout()
        {
            // Xóa tất cả dữ liệu session
            HttpContext.Session.Clear();

            // Chuyển hướng đến trang đăng nhập hoặc trang chủ
            return RedirectToAction("Login", "User");
        }

    }
}