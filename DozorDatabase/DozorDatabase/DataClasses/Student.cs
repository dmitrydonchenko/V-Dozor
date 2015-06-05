using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib.DataClasses
{
    public class Student : DataTableModel
    {
        public String FIRST_NAME { get; set; }
        public String MIDDLE_NAME { get; set; }
        public String LAST_NAME { get; set; }
        public Int32 GRADE_ID { get; set; }
        public String RFID { get; set; }


        public override string FieldsString
        {
            get { return DatabaseConstants.STUDENTS_TABLE_FIRST_NAME + "," +
                         DatabaseConstants.STUDENTS_TABLE_MIDDLE_NAME + "," +
                         DatabaseConstants.STUDENTS_TABLE_LAST_NAME + "," +
                         DatabaseConstants.STUDENTS_TABLE_GRADE_ID + "," +
                         DatabaseConstants.STUDENTS_TABLE_RFID; }
        }

        public override string ValuesParamsString
        {
            get
            {
                return "@FIRST_NAME," +
                       "@MIDDLE_NAME," +
                       "@LAST_NAME," +
                       "@GRADE_ID," +
                       "@RFID";
            }
        }

        public override List<Tuple<string, object>> ValuesList
        {
            get
            {
                return new List<Tuple<string, object>> { new Tuple<string, object>("FIRST_NAME", FIRST_NAME),
                                                         new Tuple<string, object>("MIDDLE_NAME", MIDDLE_NAME),
                                                         new Tuple<string, object>("LAST_NAME", LAST_NAME),
                                                         new Tuple<string, object>("GRADE_ID", GRADE_ID),
                                                         new Tuple<string, object>("RFID", RFID)};
            }
        }
    }
}
