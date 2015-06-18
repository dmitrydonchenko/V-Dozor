using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib.DataClasses
{
    public class Subgroup : DataTableModel 
    {
        public Int32 GRADE_ID { get; set; }
        public String SUBGROUP { get; set; }

        public override string FieldsString
        {
            get
            {
                return DatabaseConstants.SUBGROUP_TABLE_GRADE_ID + "," +
                       DatabaseConstants.SUBGROUP_TABLE_SUBGROUP;
            }
        }

        public override string ValuesParamsString
        {
            get
            {
                return "@GRADE_ID," +
                       "@SUBGROUP";
            }
        }

        public override List<Tuple<string, object>> ValuesList
        {
            get
            {
                return new List<Tuple<string, object>> { new Tuple<string, object>("GRADE_ID", GRADE_ID),
                                                         new Tuple<string, object>("SUBGROUP", SUBGROUP)};
            }
        }
    }
}
