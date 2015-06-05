using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib.DataClasses
{
    public class StudentSubgroup : DataTableModel
    {
        public String STUDENT_ID { get; set; }
        public String SUBGROUP_ID { get; set; }

        public override string FieldsString
        {
            get
            {
                return DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_STUDENT_ID + "," +
                       DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_SUBGROUP_ID;
            }
        }

        public override string ValuesParamsString
        {
            get
            {
                return "@STUDENT_ID," +
                       "@SUBGROUP_ID";
            }
        }

        public override List<Tuple<string, object>> ValuesList
        {
            get
            {
                return new List<Tuple<string, object>> { new Tuple<string, object>("STUDENT_ID", STUDENT_ID),
                                                         new Tuple<string, object>("SUBGROUP_ID", SUBGROUP_ID)};
            }
        }
    }
}
