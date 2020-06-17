using Student_Application.Entity;
using Student_Application.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Student_Application.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index(int? page)
        {
			StudentData StudentData = new StudentData();
			List<StudentDetail> studentDetails = StudentData.GetStudentDetail();

			int pageSize = 3;
			int pageNumber = (page ?? 1);
			return View(studentDetails.ToPagedList(pageNumber, pageSize));
			//return View(studentDetails);
        }
		
		public ActionResult AddStudentDetails()
		{
			return View();
		}

		[HttpPost]
		public ActionResult AddStudentDetails(StudentDetail studentDetails)
		{
			if (studentDetails != null)
			{
				StudentData studentData = new StudentData();
				string result = studentData.AddStudentDetails(studentDetails);
				if (result == "Saved")
				{
					return RedirectToAction("Index");
				}
			}
			
			return View();
		}

		[HttpPost]
		public ActionResult DeleteStudent(int ID)
		{
			if (ID != 0)
			{
				StudentData studentData = new StudentData();
				string result = studentData.DeleteStudent(ID);
			}
			return RedirectToAction("ParticipantList");
		}

		public ActionResult EditStudent(int ID)
		{
			StudentEntities entities = new StudentEntities();
			var Studentdata = entities.StudentDetails.Where(x => x.StudentDetials_ID == ID).FirstOrDefault();
			if (Studentdata != null)
			{
				return View(Studentdata);
			}
			return View();
		}

		[HttpPost]
		public ActionResult EditStudent(StudentDetail studentDetail)
		{
			StudentData studentData = new StudentData();
			string result = studentData.EditStudentDetails(studentDetail);
			if (result == "success")
			{
				return View("Index");
			}
			return View();
		}

	}
}