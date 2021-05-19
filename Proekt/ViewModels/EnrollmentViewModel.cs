using Microsoft.AspNetCore.Mvc.Rendering;
using Proekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proekt.ViewModels
{
    public class EnrollmentViewModel
    {
        public IList<Enrollment> Enrollments { get; set; }
        public SelectList Ids { get; set; }
        public string EnrollmentIds { get; set; }
        //public SelectList Years { get; set; }
        //public int EnrollmentYears { get; set; }
        public string EnrollmentCourses { get; set; }

    }
}
