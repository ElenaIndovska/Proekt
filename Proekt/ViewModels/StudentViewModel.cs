using Microsoft.AspNetCore.Mvc.Rendering;
using Proekt.Models;
using System;
using System.Collections.Generic;

namespace Proekt.ViewModels
{
    public class StudentViewModel
    {
        public IList<Student> Students { get; set; }
        public SelectList StudentId { get; set; }
        public string StudentStudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }

    }
}
