using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using DozorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DozorWeb.Controllers
{
    public class MessageController : Controller
    {
        public ActionResult MessageSending()
        {
            ViewBag.Message = "Страница для отправки сообщений.";
            MessageSendingViewModel viewModel = new MessageSendingViewModel();
            var grades = viewModel.GetAllGrades();
            viewModel.Grades = grades;
            if (grades.Count() > 0)
            {
                viewModel.Students = viewModel.GetStudents(Int32.Parse(grades.ElementAt(0).Value), -1);
                viewModel.Subgroups = viewModel.GetSubgroupsByGrade(Int32.Parse(grades.ElementAt(0).Value));
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

        public ActionResult GetStudents(int gradeId, int subgroupId)
        {
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            IEnumerable<Student> students = new List<Student>();
            if(subgroupId == -1)
            {
                students = dozorDatabase.GetStudentsByGrade(gradeId);
            }
            else
            {
                students = dozorDatabase.GetStudentsBySubgroup(subgroupId);
            }            
            List<StudentModel> studentsModels = new List<StudentModel>();
            studentsModels.Add(new StudentModel(-1, "Все ученики"));
            foreach (Student student in students)
            {
                studentsModels.Add(new StudentModel(student.ID, student.FIRST_NAME + " " +
                                                                student.MIDDLE_NAME + " " +
                                                                student.LAST_NAME));
            }
            return Json(studentsModels, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubgroups(int gradeId)
        {
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            IEnumerable<Subgroup> subgroups = dozorDatabase.GetSubgroupsByGradeId(gradeId);
            List<SubgroupModel> subgroupsModels = new List<SubgroupModel>();
            subgroupsModels.Add(new SubgroupModel(-1, "Все ученики"));
            foreach (Subgroup subgroup in subgroups)
            {
                subgroupsModels.Add(new SubgroupModel(subgroup.ID, subgroup.SUBGROUP));
            }

            JsonResult jsonResult = new JsonResult();
            jsonResult = Json(subgroupsModels, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public JsonResult SendMessage(int gradeId, int subgroupId, int studentId, String messageText, DateTime messageExpirationDateTme, String selectedMessageShowDirection)
        {
            if (messageText == null || (gradeId == -1 && studentId == -1 && subgroupId == -1))
                return Json("Выберите адресата сообщения", JsonRequestBehavior.AllowGet);           
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            Boolean res = true;
            if(messageExpirationDateTme == null)
            {
                messageExpirationDateTme = DateTime.Now;                
            }
            messageExpirationDateTme = messageExpirationDateTme.AddHours(23 - messageExpirationDateTme.Hour);
            messageExpirationDateTme = messageExpirationDateTme.AddMinutes(59 - messageExpirationDateTme.Minute);
            int showDirection = 0;
            if (selectedMessageShowDirection == "На входе")
                showDirection = 1;
            else if (selectedMessageShowDirection == "На выходе")
                showDirection = 2;
            if(studentId != -1)
            {
                Message message = new Message();
                message.DATETIME = DateTime.Now;
                message.MESSAGE_TEXT = messageText;
                message.STUDENT_ID = studentId;
                message.MESSAGE_PRIORITY = 2;
                message.EXPIRATION_DATETIME = messageExpirationDateTme;
                message.MESSAGE_SHOW_DIRECTION = showDirection;
                res &= dozorDatabase.InsertMessage(message);
            }
            else if(subgroupId != -1)
            {
                IEnumerable<Student> students = dozorDatabase.GetStudentsBySubgroup(subgroupId);
                foreach(Student student in students)
                {
                    Message message = new Message();
                    message.DATETIME = DateTime.Now;
                    message.MESSAGE_TEXT = messageText;
                    message.STUDENT_ID = student.ID;
                    message.MESSAGE_PRIORITY = 1;
                    message.EXPIRATION_DATETIME = messageExpirationDateTme;
                    message.MESSAGE_SHOW_DIRECTION = showDirection;
                    res &= dozorDatabase.InsertMessage(message);
                }
            }
            else if(gradeId != -1)
            {
                IEnumerable<Student> students = dozorDatabase.GetStudentsByGrade(gradeId);
                foreach (Student student in students)
                {
                    Grade grade = dozorDatabase.GetGradeById(gradeId);
                    if (grade == null)
                        return Json(false);
                    String gradeName = grade.GRADE;
                    Message message = new Message();
                    message.DATETIME = DateTime.Now;
                    message.MESSAGE_TEXT = messageText;
                    message.STUDENT_ID = student.ID;
                    message.MESSAGE_PRIORITY = 1;
                    message.EXPIRATION_DATETIME = messageExpirationDateTme;
                    message.MESSAGE_SHOW_DIRECTION = showDirection;
                    res &= dozorDatabase.InsertMessage(message);
                }   
            }
            if(res)
            {
                return Json("Сообщение успешно сохранено", JsonRequestBehavior.AllowGet);
            }
            return Json("Ошибка при выполнения запроса", JsonRequestBehavior.AllowGet);
        }
    }
}