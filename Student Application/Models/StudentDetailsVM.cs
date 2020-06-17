using Student_Application.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Student_Application.Model
{
	public class StudentDetailsVM
	{
		public int StudentDetials_ID { get; set; }
		public int Student_ID { get; set; }
		public Nullable<decimal> Marks { get; set; }
		public int Subject_ID { get; set; }
		public string First_Name { get; set; }
		public string Last_Name { get; set; }
		public string Class { get; set; }

		public string Subject_Name { get; set; }

		public virtual Student Student { get; set; }
		public virtual Subject Subject { get; set; }
	}
}