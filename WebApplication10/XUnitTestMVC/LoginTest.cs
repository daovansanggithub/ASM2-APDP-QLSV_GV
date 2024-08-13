using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using WebApplication10.Controllers;
using WebApplication10.Models;
using System.Linq.Expressions;

namespace XUnitTestMVC
{
    public class UserControllerTests
    {
        private readonly Mock<QlsvMvc2Context> _mockContext;
        private readonly Mock<DbSet<User>> _mockUsersDbSet;
        private readonly Mock<DbSet<Teacher>> _mockTeachersDbSet;
        private readonly Mock<ISession> _mockSession;
        private readonly Mock<HttpContext> _mockHttpContext;

        public UserControllerTests()
        {
            _mockContext = new Mock<QlsvMvc2Context>();
            _mockUsersDbSet = new Mock<DbSet<User>>();
            _mockTeachersDbSet = new Mock<DbSet<Teacher>>();
            _mockSession = new Mock<ISession>();
            _mockHttpContext = new Mock<HttpContext>();

            _mockHttpContext.Setup(s => s.Session).Returns(_mockSession.Object);
        }

        [Fact]
        public async Task Login_ValidAdminUser_RedirectsToAdminIndex()
        {
            // Arrange
            var loginModel = new LoginViewModel { Username = "admin", Password = "admin" };
            var user = new User { UserId = 1, UserName = "admin", Password = "admin", Role = "admin" };

            _mockUsersDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
                .ReturnsAsync(user);
            _mockContext.Setup(c => c.Users).Returns(_mockUsersDbSet.Object);

            var controller = new UserController(_mockContext.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object
                }
            };

            // Act
            var result = await controller.Login(loginModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("CRUD_User", result.ControllerName);
        }

        [Fact]
        public async Task Login_ValidTeacherUser_RedirectsToTeacherIndex()
        {
            // Arrange
            var loginModel = new LoginViewModel { Username = "dong", Password = "dong" };
            var user = new User { UserId = 2, UserName = "dong", Password = "dong", Role = "teacher" };
            var teacher = new Teacher { TeacherId = 3, UserId = 2 };

            _mockUsersDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
                .ReturnsAsync(user);
            _mockTeachersDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), default))
                .ReturnsAsync(teacher);
            _mockContext.Setup(c => c.Users).Returns(_mockUsersDbSet.Object);
            _mockContext.Setup(c => c.Teachers).Returns(_mockTeachersDbSet.Object);

            var controller = new UserController(_mockContext.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object
                }
            };

            // Act
            var result = await controller.Login(loginModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TeacherIndex", result.ActionName);
            Assert.Equal("Teacher", result.ControllerName);
        }

        [Fact]
        public async Task Login_InvalidUser_ReturnsViewWithError()
        {
            // Arrange
            var loginModel = new LoginViewModel { Username = "sang", Password = "sang" };

            _mockUsersDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
                .ReturnsAsync((User)null);
            _mockContext.Setup(c => c.Users).Returns(_mockUsersDbSet.Object);

            var controller = new UserController(_mockContext.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _mockHttpContext.Object
                }
            };

            // Act
            var result = await controller.Login(loginModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(controller.ViewBag.err == "Tài khoản không tồn tại.");
        }
    }
}
