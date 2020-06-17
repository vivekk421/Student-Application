using Student_Application.Entity;
using Student_Application.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Student_Application
{
	public class StudentData
	{
		public string AddStudentDetails(StudentDetail studentDetails)
		{
			using (var dbContext = new StudentEntities())
			{
				using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
				{
					try
					{
						var student = new Student()
						{
							First_Name = studentDetails.First_Name,
							Last_Name = studentDetails.Last_Name,
							Class = studentDetails.Class
						};
						dbContext.Students.Add(student);
						dbContext.SaveChanges();
						int Student_ID = student.Student_ID;
						var subject = new Subject()
						{
							Subject_Name = studentDetails.Subject_Name
						};
						dbContext.Subjects.Add(subject);
						dbContext.SaveChanges();
						int SubjectID = subject.SubjectID;
						var studentDetail = new StudentDetail()
						{
							Student_ID = Student_ID,
							Subject_ID = SubjectID,
							Marks = studentDetails.Marks
						};
						dbContext.StudentDetails.Add(studentDetail);
						dbContext.SaveChanges();
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						return ex.Message;
					}
				}
			}
			return "Saved";
		}

		public List<StudentDetail> GetStudentDetail()
		{
			using (var dbContext = new StudentEntities())
			{
				var students = (from p in dbContext.StudentDetails
								join f in dbContext.Students
								on p.Student_ID equals f.Student_ID
								join s in dbContext.Subjects
								on p.Subject_ID equals s.SubjectID
								select new
								{
									StudentDetials_ID = p.StudentDetials_ID,
									Marks = p.Marks,
									//Class = p.Class,
									First_Name = f.First_Name,
									Last_Name = f.Last_Name,
									Subject_Name = s.Subject_Name
								}).ToList().Select(x => new StudentDetail()
								{
									First_Name = x.First_Name,
									Last_Name = x.Last_Name,
									Marks = x.Marks,
									//Class = x.Class,
									Subject_Name = x.Subject_Name,
									StudentDetials_ID = x.StudentDetials_ID
								});
				return students.ToList();
			}
		}

		public string DeleteStudent(int ID)
		{
			using (var dbContext = new StudentEntities())
			{
				using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
				{
					try
					{
						var studentDetails = (from b in dbContext.StudentDetails
											where b.StudentDetials_ID == ID
											select b).FirstOrDefault();
						dbContext.StudentDetails.Remove(studentDetails);

						var students = (from b in dbContext.Students
										where b.Student_ID == studentDetails.Student_ID
										select b).ToList();
						foreach (var item in students)
						{
							dbContext.Students.Remove(item);
						}

						var subjects = (from b in dbContext.Subjects
										where b.SubjectID == studentDetails.Subject_ID
										select b).ToList();
						foreach (var item in subjects)
						{
							dbContext.Subjects.Remove(item);
						}					

						dbContext.SaveChanges();

						transaction.Commit();
						return "success";
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						return ex.Message;
					}
				}
			}
		}

		public string EditStudentDetails(StudentDetail detail)
		{
			using (var dbContext = new StudentEntities())
			{
				using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
				{
					try
					{
						var studentDetails = dbContext.StudentDetails.Where(x => x.StudentDetials_ID == detail.StudentDetials_ID).FirstOrDefault();
						var student = dbContext.Students.Where(x => x.Student_ID == studentDetails.Student_ID).FirstOrDefault();
						var subject = dbContext.Subjects.Where(x => x.SubjectID == studentDetails.Subject_ID).FirstOrDefault();
						if (studentDetails != null)
						{
							studentDetails.Marks = detail.Marks;
							studentDetails.Class = detail.Class;
							student.First_Name = detail.First_Name;
							student.Last_Name = detail.Last_Name;
							subject.Subject_Name = detail.Subject_Name;
							dbContext.Entry(studentDetails).State = EntityState.Modified;
							dbContext.Entry(student).State = EntityState.Modified;
							dbContext.Entry(subject).State = EntityState.Modified;
							dbContext.SaveChanges();
							transaction.Commit();
						}						
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						return ex.Message;
					}
				}
			}
			return "success";
		}
	}
}