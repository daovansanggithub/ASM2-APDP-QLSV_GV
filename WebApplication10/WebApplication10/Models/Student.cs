using System;
using System.Collections.Generic;

namespace WebApplication10.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? StudentName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    /*public int? UserId { get; set; }*/

    public virtual ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();

    /*public virtual User User { get; set; } = null!;*/
}
