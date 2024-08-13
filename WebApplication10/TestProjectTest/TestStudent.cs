using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication10.Controllers;
using WebApplication10.Models;
using Xunit;

public class StudentsControllerTests
{
    private readonly Mock<DbSet<Student>> _mockSet;
    private readonly Mock<QlsvMvc2Context> _mockContext;
    private readonly StudentsController _controller;

    public StudentsControllerTests()
    {
        _mockSet = new Mock<DbSet<Student>>();
        _mockContext = new Mock<QlsvMvc2Context>();
        _controller = new StudentsController(_mockContext.Object);

        var students = new List<Student>
        {
            new Student { StudentId = 1, StudentName = "John Doe", Address = "123 Street", Phone = "1234567890" },
            new Student { StudentId = 2, StudentName = "Jane Smith", Address = "456 Avenue", Phone = "0987654321" }
        }.AsQueryable();

        _mockSet.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(students.Provider);
        _mockSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(students.Expression);
        _mockSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(students.ElementType);
        _mockSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(students.GetEnumerator());

        _mockContext.Setup(c => c.Students).Returns(_mockSet.Object);
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithAListOfStudents()
    {
        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Student>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Details_ReturnsNotFound_WhenIdIsNull()
    {
        // Act
        var result = await _controller.Details(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ReturnsNotFound_WhenStudentNotFound()
    {
        // Act
        var result = await _controller.Details(3);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ReturnsAViewResult_WithAStudent()
    {
        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Student>(viewResult.ViewData.Model);
        Assert.Equal("John Doe", model.StudentName);
    }
}
