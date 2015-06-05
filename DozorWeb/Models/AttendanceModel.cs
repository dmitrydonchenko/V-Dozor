using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using DozorMediaLib.Video;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace DozorWeb.Models
{
    public class AttendanceModel
    {
        public String AttendanceTime { get; set; }
        public String Snapshot { get; set; }
        public String Direction { get; set; }

        public AttendanceModel(DateTime attendanceDateTime, Byte[] snapshot, Boolean isIn)
        {
            AttendanceTime = attendanceDateTime.ToShortTimeString();
            Snapshot = Convert.ToBase64String(snapshot);
            if(isIn)
            {
                Direction = "Вход";
            }
            else
            {
                Direction = "Выход";
            }
        }
    }
}