using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Proekt.Areas.Identity.Data;
using Proekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proekt.Models
{
    public class SeedData
    {

        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ProektUser>>();
            IdentityResult roleResult;
            // Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }

            ProektUser user = await UserManager.FindByEmailAsync("admin@proekt.com");


            if (user == null)
            {
                var User = new ProektUser();

                User.Email = "admin@proekt.com";
                User.UserName = "admin@proekt.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(User, "Admin");
                }
            }

        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ProektContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ProektContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                if(context.Course.Any() || context.Student.Any() || context.Teacher.Any())
                {
                    return;
                }

                context.Teacher.AddRange(
                    new Teacher { /*Id=1*/ FirstName = "Pero", LastName = "Latkoski", Degree = "PhD", AcademicRank = "Professor" },
                    new Teacher { /*Id=2*/ FirstName = "Daniel", LastName = "Denkovski", Degree = "PhD", AcademicRank = "Asst. Prof" },
                    new Teacher { /*Id=3*/ FirstName = "Goran", LastName = "Jakimovski", Degree = "PhD", AcademicRank = "Asst. Prof" }
                );

                context.SaveChanges();

                context.Student.AddRange(
                    new Student { StudentId = "184/2017", FirstName = "Elena", LastName = "Indovska", EntollmentDate = DateTime.Parse("2017-09-15"), AcquiredCredits = 162, CurrentSemester = 8, EducationLevel = "Student" },
                    new Student { StudentId = "31/2019", FirstName = "Nikola", LastName = "Indovski", EntollmentDate = DateTime.Parse("2019-09-15"), AcquiredCredits = 50, CurrentSemester = 4, EducationLevel = "Student" },
                    new Student { StudentId = "193/2017", FirstName = "Martina", LastName = "Markovska", EntollmentDate = DateTime.Parse("2017-09-15"), AcquiredCredits = 195, CurrentSemester = 8, EducationLevel = "Student" }
                );

                context.SaveChanges();

                context.Course.AddRange(
                    new Course
                    {
                        /*Id=1*/
                        Title = "Razvoj na serverski WEB aplikacii",
                        Credits = 6,
                        Semester = 6,
                        Programme = "KTI, TKII",
                        EducationLevel = "Undergraduate",
                        FirstTeacherID = context.Teacher.Single(t => t.FirstName == "Pero" && t.LastName == "Latkoski").Id,
                        SecondTeacherID = context.Teacher.Single(t => t.FirstName == "Daniel" && t.LastName == "Denkovski").Id
                    },

                    new Course
                    {
                        /*Id=2*/
                        Title = "Modeliranje na podatoci i bazi",
                        Credits = 6,
                        Semester = 4,
                        Programme = "KTI, TKII, KHIE, KSIAR",
                        EducationLevel = "Undergraduate",
                        FirstTeacherID = context.Teacher.Single(t => t.FirstName == "Goran" && t.LastName == "Jakimovski").Id
                    },

                    new Course
                    {
                        /*Id = 3*/
                        Title = "Android",
                        Credits = 6,
                        Semester = 8,
                        Programme = "KTI, TKII",
                        EducationLevel = "Undergraduate",
                        FirstTeacherID = context.Teacher.Single(t => t.FirstName == "Pero" && t.LastName == "Latkoski").Id,
                        SecondTeacherID = context.Teacher.Single(t => t.FirstName == "Daniel" && t.LastName == "Denkovski").Id

                    }
                );

                context.SaveChanges();

                context.Enrollment.AddRange(
                    new Enrollment { CourseId = 1, StudentId = 1, Year = 2017, Semester = "8", ExamPoints = 0 },
                    new Enrollment { CourseId = 2, StudentId = 1, Year = 2017, Semester = "6", ExamPoints = 60 },
                    new Enrollment { CourseId = 1, StudentId = 3, Year = 2017, Semester = "8", ExamPoints = 100 },
                    new Enrollment { CourseId = 2, StudentId = 3, Year = 2017, Semester = "4", ExamPoints = 80 },
                    new Enrollment { CourseId = 2, StudentId = 2, Year = 2019, Semester = "4", ExamPoints = 100 }
                );

                context.SaveChanges();

            }
        }
    }
}
