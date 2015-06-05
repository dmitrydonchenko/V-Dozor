using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DozorWeb.Models
{
    public class StudentModel
    {
        public Int32 StudentId { get; set; }
        public String Name { get; set; }

        public StudentModel(Int32 studentId, String name)
        {
            StudentId = studentId;
            Name = name;
        }        
    }
}