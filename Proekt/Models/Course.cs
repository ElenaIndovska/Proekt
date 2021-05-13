using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proekt.Models
{
    public class Course
    {
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Title { get; set; }

		public int Credits { get; set; }
		public int Semester { get; set; }

		[StringLength(100)]
		public string Programme { get; set; }

		[StringLength(25)]
		[Display(Name = "Education Level")]
		public string EducationLevel { get; set; }

		[Display(Name = "First Teacher")]
		[ForeignKey("FirstTeacherID")]
		public int? FirstTeacherID { get; set; }
		public Teacher FirstTeacher { get; set; }

		[Display(Name = "Second Teacher")]
		[ForeignKey("SecondTeacherID")]
		public int? SecondTeacherID { get; set; }
		public Teacher SecondTeacher { get; set; }

		public ICollection<Enrollment> Students { get; set; }
	}
}
