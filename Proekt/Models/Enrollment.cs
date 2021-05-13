using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proekt.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }


        public int StudentId { get; set; }
        public Student Student { get; set; }


        [StringLength(10)]
        public string Semester { get; set; }

        public int? Year { get; set; }

        public int? Grade { get; set; }

        [StringLength(255)]
        [Display(Name = "Seminal Url")]
        public string SeminalUrl { get; set; }

        [StringLength(255)]
        [Display(Name = "Project Url")]
        public string ProjectUrl { get; set; }

        [Display(Name = "Exam Points")]
        public int? ExamPoints { get; set; }

        [Display(Name = "Seminal Points")]
        public int? SeminalPoints { get; set; }

        [Display(Name = "Project Points")]
        public int? ProjectPoints { get; set; }

        [Display(Name = "Additional Points")]
        public int? AdditionalPoints { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishDate { get; set; }
    }
}
