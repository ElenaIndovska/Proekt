using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proekt.Models
{
    public class Student
    {
		public int Id { get; set; }

		[Required]
		[Display(Name = "Index")]
		[StringLength(10)]
		public string StudentId { get; set; }

		[Required]
		[Display(Name = "First Name")]
		[StringLength(50)]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		[StringLength(50)]
		public string LastName { get; set; }

		[Display(Name = "Enrollment Date")]
		[DataType(DataType.Date)]
		public DateTime? EntollmentDate { get; set; }


		[Display(Name = "Acquired Credits")]
		public int? AcquiredCredits { get; set; }

		[Display(Name = "Current Semester")]
		public int? CurrentSemester { get; set; }

		[Display(Name = "Education Level")]
		[StringLength(25)]
		public string EducationLevel { get; set; }

		public string FullName
		{
			get { return String.Format("{0} {1}", FirstName, LastName); }
		}

		public ICollection<Enrollment> Courses { get; set; }
	}
}
