using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proekt.Data;
using Proekt.Models;
using Proekt.ViewModels;

namespace Proekt.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ProektContext _context;

        public CoursesController(ProektContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int? courseSemester, string courseProgramme, string searchString)
        {
            //var proektContext = _context.Course.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).Include(c => c.Students).ThenInclude(c => c.Student);
            //return View(await proektContext.ToListAsync());
            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<int> courseSemesterQuery = _context.Course.OrderBy(c => c.Semester).Select(c => c.Semester).Distinct();
            IQueryable<string> courseProgrammeQuery = _context.Course.OrderBy(c => c.Programme).Select(c => c.Programme).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(c => c.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(courseProgramme))
            {
                courses = courses.Where(c => c.Programme == courseProgramme);
            }

            if (courseSemester != null)
            {
                courses = courses.Where(c => c.Semester == courseSemester);
            }

            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).Include(c => c.Students).ThenInclude(c => c.Student);

            var courseVM = new CourseViewModel
            {
                Semester = new SelectList(await courseSemesterQuery.ToListAsync()),
                Programme = new SelectList(await courseProgrammeQuery.ToListAsync()),
                Courses = await courses.ToListAsync()
            };

            return View(courseVM);

        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["FirstTeacherID"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName");
            ViewData["SecondTeacherID"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherID,SecondTeacherID")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherID"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.FirstTeacherID);
            ViewData["SecondTeacherID"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.SecondTeacherID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FirstTeacherID"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.FirstTeacherID);
            ViewData["SecondTeacherID"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.SecondTeacherID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherID,SecondTeacherID")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["FirstTeacherID"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.FirstTeacherID);
            ViewData["SecondTeacherID"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.SecondTeacherID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Students(int? id)
        {
            IQueryable<Student> students = _context.Student.AsQueryable();
            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();

            enrollments = enrollments.Where(e => e.CourseId == id);

            IEnumerable<int> enrollStudentId = enrollments.Select(e => e.StudentId).Distinct();

            students = students.Where(s => enrollStudentId.Contains(s.Id));
            students = students.Include(s => s.Courses).ThenInclude(s => s.Course);

            ViewData["CourseTitle"] = _context.Course.Where(c => c.Id == id).Select(c => c.Title).FirstOrDefault();
            ViewData["CourseId"] = id;

            return View(students);
        }

    }
}
