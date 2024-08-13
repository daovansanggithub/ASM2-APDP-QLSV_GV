using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication10.Models;

public partial class QlsvMvc2Context : DbContext
{
    public QlsvMvc2Context()
    {
    }

    public QlsvMvc2Context(DbContextOptions<QlsvMvc2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseAssignment> CourseAssignments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-Q0PAQT9D\\SANGHOCSQL;Initial Catalog=QLSV-mvc2;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseAssignment>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_CourseAssignments_CourseId");

            entity.HasIndex(e => e.StudentId, "IX_CourseAssignments_StudentId");

            entity.HasIndex(e => e.TeacherId, "IX_CourseAssignments_TeacherId");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseAssignments).HasForeignKey(d => d.CourseId);

            entity.HasOne(d => d.Student).WithMany(p => p.CourseAssignments).HasForeignKey(d => d.StudentId);

            entity.HasOne(d => d.Teacher).WithMany(p => p.CourseAssignments).HasForeignKey(d => d.TeacherId);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Teachers_UserId").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.Teacher).HasForeignKey<Teacher>(d => d.UserId);
        });



        /*modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Students_UserId");

            entity.HasOne(d => d.User)
                .WithMany(u => u.Students) // Mối quan hệ một-nhiều với User
                .HasForeignKey(d => d.UserId);
        });*/

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Role).HasDefaultValue("");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
