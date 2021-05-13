using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Proekt.Models
{
    public class Teacher
    {
		public int Id { get; set; }

		[Required]
		[StringLength(50)]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required]
		[StringLength(50)]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[StringLength(50)]
		public string Degree { get; set; }

		[StringLength(25)]
		[Display(Name = "Academic Rank")]
		public string AcademicRank { get; set; }

		[StringLength(10)]
		[Display(Name = "Office Number")]
		public string OfficeNumber { get; set; }

		[Display(Name = "Hire Date")]
		[DataType(DataType.Date)]
		public DateTime? HireDate { get; set; }

		public string FullName
		{
			get { return String.Format("{0} {1}", FirstName, LastName); }
		}

		[InverseProperty("FirstTeacher")]
		public ICollection<Course> FirstCourse { get; set; }

		[InverseProperty("SecondTeacher")]
		public ICollection<Course> SecondCourse { get; set; }

	}
}
