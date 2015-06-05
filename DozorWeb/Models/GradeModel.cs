using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DozorWeb.Models
{
    public class GradeModel
    {
        public Int32 GradeId { get; set; }
        public String Grade { get; set; }

        public GradeModel(int gradeId, String grade)
        {
            GradeId = gradeId;
            Grade = grade;
        }        
    }
}