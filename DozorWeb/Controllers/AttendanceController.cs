using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using DozorMediaLib.Video;
using DozorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DozorWeb.Controllers
{
    public class AttendanceController : Controller
    {
        public ActionResult Attendancies()
        {
            ViewBag.Message = "Страница для просмотра посещений.";
            AttendanciesPageViewModel viewModel = new AttendanciesPageViewModel();
            var grades = viewModel.GetAllGrades();
            viewModel.Grades = grades;
            if(grades.Count() > 0)
            {
                viewModel.Students = viewModel.GetStudentsByGrade(0);                   
            }
            return View(viewModel);
        }

        private IEnumerable<SelectListItem> GetGrades()
        {
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            IEnumerable<Grade> grades = dozorDatabase.GetAllGrades();
            List<GradeModel> gradesModels = new List<GradeModel>();
            foreach (Grade grade in grades)
            {
                gradesModels.Add(new GradeModel(grade.ID, grade.GRADE));
            }
            ViewData["Grades"] = new SelectList(gradesModels,
                                  "GradeId",
                                  "Grade");
            return new SelectList(gradesModels,
                                  "GradeId",
                                  "Grade");
        }

        public ActionResult GetStudents(int gradeId)
        {
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            IEnumerable<Student> students = dozorDatabase.GetStudentsByGrade(gradeId);
            List<StudentModel> studentsModels = new List<StudentModel>();
            foreach (Student student in students)
            {
                studentsModels.Add(new StudentModel(student.ID, student.FIRST_NAME + " " +
                                                                student.MIDDLE_NAME + " " +
                                                                student.LAST_NAME));
            }
            return Json(studentsModels, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAttendancies(int studentId, DateTime attendanciesDateTime)
        {
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            IEnumerable<Attendance> attendancies = dozorDatabase.GetStudentAttendanciesByDate(studentId, attendanciesDateTime);
            List<AttendanceModel> attendanciesModels = new List<AttendanceModel>();
            foreach(Attendance attendance in attendancies)
            {
                attendanciesModels.Add(new AttendanceModel(attendance.DATETIME, attendance.SNAPSHOT, attendance.IS_IN));
            }
            
            JsonResult jsonResult = new JsonResult();            
            jsonResult = Json(attendanciesModels, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = Int32.MaxValue;
            //string str = new JavaScriptSerializer().Serialize(jsonResult.Data);
            return jsonResult;
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
    }
}