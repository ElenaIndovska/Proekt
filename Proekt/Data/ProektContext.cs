using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proekt.Areas.Identity.Data;
using Proekt.Models;

namespace Proekt.Data
{
    public class ProektContext : IdentityDbContext<ProektUser>
    {
        public ProektContext (DbContextOptions<ProektContext> options)
            : base(options)
        {
        }

        public DbSet<Proekt.Models.Course> Course { get; set; }

        public DbSet<Proekt.Models.Student> Student { get; set; }

        public DbSet<Proekt.Models.Teacher> Teacher { get; set; }
        public DbSet<Proekt.Models.Enrollment> Enrollment { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Enrollment>()
                .HasOne<Student>(s => s.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(s => s.StudentId);

            builder.Entity<Enrollment>()
                .HasOne<Course>(c => c.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(c => c.CourseId);

            builder.Entity<Course>()
                .HasOne<Teacher>(t => t.FirstTeacher)
                .WithMany(t => t.FirstCourse)
                .HasForeignKey(t => t.FirstTeacherID);

            builder.Entity<Course>()
                .HasOne<Teacher>(t => t.SecondTeacher)
                .WithMany(t => t.SecondCourse)
                .HasForeignKey(t => t.SecondTeacherID);
        }
    }
}
