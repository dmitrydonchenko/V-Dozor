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
        // GET: Message
        public ActionResult MessageSending(string button, int gradeId = 0, int studentId = -1, int duration = 1, String messageText = "")
        {
            ViewBag.Message = "Страница для просмотра и отправки сообщений.";
            MessageSendingViewModel viewModel = new MessageSendingViewModel();
            viewModel.SelectedGradeId = gradeId;
            viewModel.Grades = viewModel.GetAllGrades();
            viewModel.Students = viewModel.GetStudentsByGrade(gradeId);
            if(button == "submit")
            {
                Message message = new Message();
                message.IS_PERSONAL_MESSAGE = studentId == -1 ? false : true;
                if (message.IS_PERSONAL_MESSAGE)
                {
                    message.STUDENT_ID = studentId;
                    message.GRADE_ID = -1;
                }
                else
                {
                    message.STUDENT_ID = -1;
                    message.GRADE_ID = gradeId;
                }
                message.MESSAGE_TEXT = messageText;
                message.DATETIME = DateTime.Now;
                message.EXPIRATION_DATETIME = DateTime.Now.AddDays(duration);
                DozorDatabase dozorDatabase = DozorDatabase.Instance;
                if(dozorDatabase.InsertMessage(message))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(viewModel);
        }
    }
}