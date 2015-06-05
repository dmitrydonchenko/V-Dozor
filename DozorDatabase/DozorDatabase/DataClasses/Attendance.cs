using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib.DataClasses
{
    public class Attendance : DataTableModel
    {
        public DateTime DATETIME { get; set; }
        public Int32 STUDENT_ID { get; set; }
        public Byte[] SNAPSHOT { get; set; }
        public Boolean IS_IN { get; set; }

        public override string FieldsString
        {
            get
            {
                return DatabaseConstants.ATTENDANCE_TABLE_DATETIME + "," +
                       DatabaseConstants.ATTENDANCE_TABLE_STUDENT_ID + "," +
                       DatabaseConstants.ATTENDANCE_TABLE_SNAPSHOT + "," +
                       DatabaseConstants.ATTENDANCE_TABLE_IS_IN;
            }
        }

        public override string ValuesParamsString
        {
            get
            {
                return "@DATETIME," + 
                       "@STUDENT_ID," +
                       "@SNAPSHOT," +
                       "@IS_IN"; 
            }
        }

        public override List<Tuple<string, object>> ValuesList
        {
            get { return new List<Tuple<string, object>> { new Tuple<string, object>("DATETIME", DATETIME),
                                                           new Tuple<string, object>("STUDENT_ID", STUDENT_ID),
                                                           new Tuple<string, object>("SNAPSHOT", SNAPSHOT),
                                                           new Tuple<string, object>("IS_IN", IS_IN)};
            }
        }
    }
}
