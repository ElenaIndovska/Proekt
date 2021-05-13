using Microsoft.AspNetCore.Mvc.Rendering;
using Proekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proekt.ViewModels
{
    public class CourseViewModel
    {
        public IList<Course> Courses { get; set; }
        public SelectList Semester { get; set; }
        public int CourseSemester { get; set; }
        public SelectList Programme { get; set; }
        public string CourseProgramme { get; set; }
        public string SearchString { get; set; }

    }
}
