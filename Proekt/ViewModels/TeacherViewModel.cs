using Microsoft.AspNetCore.Mvc.Rendering;
using Proekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proekt.ViewModels
{
    public class TeacherViewModel
    {
        public IList <Teacher> Teachers { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public SelectList Degree { get; set; }
        public string TeacherDegree { get; set; }
        public SelectList AcademicRank { get; set; }
        public string TeacherAcademicRank { get; set; }

    }
}
