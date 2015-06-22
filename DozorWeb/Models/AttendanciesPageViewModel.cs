using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DozorWeb.Models
{
    public class AttendanciesPageViewModel
    {
        public Int32 SelectedGradeId { get; set; }
        public Int32 SelectedStudentId { get; set; }
        public String AttendanciesDateTime { get; set; }

        public SelectList Grades { get; set; }
        public IEnumerable<SelectListItem> Students { get; set; }
        public IEnumerable<AttendanceModel> Attendancies { get; set; }

        public AttendanciesPageViewModel()
        {
            Students = Enumerable.Empty<SelectListItem>();
            SelectedStudentId = 0;
            AttendanciesDateTime = DateTime.Now.ToString("MM/dd/yyyy");
        }

        public SelectList GetAllGrades()
        {
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            IEnumerable<Grade> grades = dozorDatabase.GetAllGrades();
            List<GradeModel> gradesModels = new List<GradeModel>();
            foreach (Grade grade in grades)
            {
                gradesModels.Add(new GradeModel(grade.ID, grade.GRADE));
            }
            return new SelectList(gradesModels,
                                  "GradeId",
                                  "Grade", SelectedGradeId);
        }       

        public SelectList GetStudentsByGrade(int gradeId)
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
            return new SelectList(studentsModels,
                                  "StudentId",
                                  "Name");
        }
    }
}