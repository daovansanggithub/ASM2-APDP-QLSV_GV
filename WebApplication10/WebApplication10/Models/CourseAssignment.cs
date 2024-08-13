using System;
using System.Collections.Generic;

namespace WebApplication10.Models;

public partial class CourseAssignment
{
    public int TeacherId { get; set; }

    public int CourseId { get; set; }

    public int StudentId { get; set; }

    public int CourseAssignmentId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
