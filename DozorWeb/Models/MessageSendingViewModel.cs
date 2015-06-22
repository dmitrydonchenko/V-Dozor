using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DozorWeb.Models
{
    public class MessageSendingViewModel
    {
        public Int32 SelectedGradeId { get; set; }
        public Int32 SelectedSubgroupId { get; set; }
        public Int32 SelectedStudentId { get; set; }
        public String MessageText { get; set; }

        public SelectList Grades { get; set; }
        public SelectList Students { get; set; }
        public SelectList Subgroups { get; set; }

        public MessageSendingViewModel()
        {
            SelectedGradeId = -1;
            SelectedStudentId = -1;
            SelectedSubgroupId = -1;
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

        public SelectList GetStudents(int gradeId, int subgroupId)
        {
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            IEnumerable<Student> students;
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
            return new SelectList(studentsModels,
                                  "StudentId",
                                  "Name");
        }

        public SelectList GetSubgroupsByGrade(int gradeId)
        {
            DozorDatabase dozorDatabase = DozorDatabase.Instance;
            IEnumerable<Subgroup> subgroups = dozorDatabase.GetSubgroupsByGradeId(gradeId);
            List<SubgroupModel> subgroupsModels = new List<SubgroupModel>();
            subgroupsModels.Add(new SubgroupModel(-1, "Все ученики"));
            foreach (Subgroup subgroup in subgroups)
            {
                subgroupsModels.Add(new SubgroupModel(subgroup.ID, subgroup.SUBGROUP));
            }
            return new SelectList(subgroupsModels,
                                  "SubgroupId",
                                  "Subgroup", SelectedSubgroupId);
        }
    }
}