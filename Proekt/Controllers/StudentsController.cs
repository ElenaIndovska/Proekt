using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proekt.Data;
using Proekt.Models;
using Proekt.ViewModels;

namespace Proekt.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ProektContext _context;

        public StudentsController(ProektContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string studentStudentId, string studentFirstName, string studentLastName)
        {
            //return View(await _context.Student.ToListAsync());
            IQueryable<Student> students = _context.Student.AsQueryable();
            IQueryable<string> studentIdQuery = _context.Student.OrderBy(s => s.StudentId).Select(s => s.StudentId).Distinct();

            if (!string.IsNullOrEmpty(studentFirstName))
            {
                students = students.Where(s => s.FirstName.Contains(studentFirstName));
            }

            if (!string.IsNullOrEmpty(studentLastName))
            {
                students = students.Where(s => s.LastName.Contains(studentLastName));
            }

            if (!string.IsNullOrEmpty(studentStudentId))
            {
                students = students.Where(s => s.StudentId == studentStudentId);
            }

            var studentVM = new StudentViewModel
            {
                StudentId = new SelectList(await studentIdQuery.ToListAsync()),
                Students = await students.ToListAsync()
            };

            return View(studentVM);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,StudentId,FirstName,LastName,EntollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,FirstName,LastName,EntollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Courses(int? id)
        {
            IQueryable<Course> courses = _context.Course.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).AsQueryable();
            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();

            enrollments = enrollments.Where(e => e.StudentId == id);

            IEnumerable<int> enrollCourseId = enrollments.Select(e => e.CourseId).Distinct();

            courses = courses.Where(c => enrollCourseId.Contains(c.Id));
            courses = courses.Include(c => c.Students).ThenInclude(c => c.Student);

            ViewData["StudentFullName"] = _context.Student.Where(t => t.Id == id).Select(t => t.FullName).FirstOrDefault();
            ViewData["StudentId"] = id;
            return View(courses);

        }
    }
}
