using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib.DataClasses
{
    public class Subgroup : DataTableModel 
    {
        public String GRADE_ID { get; set; }
        public String NAME { get; set; }

        public override string FieldsString
        {
            get
            {
                return DatabaseConstants.SUBGROUP_TABLE_GRADE_ID + "," +
                       DatabaseConstants.SUBGROUP_TABLE_NAME;
            }
        }

        public override string ValuesParamsString
        {
            get
            {
                return "@GRADE_ID," +
                       "@NAME";
            }
        }

        public override List<Tuple<string, object>> ValuesList
        {
            get
            {
                return new List<Tuple<string, object>> { new Tuple<string, object>("GRADE_ID", GRADE_ID),
                                                         new Tuple<string, object>("NAME", NAME)};
            }
        }
    }
}
