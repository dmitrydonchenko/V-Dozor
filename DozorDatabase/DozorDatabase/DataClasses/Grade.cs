using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib.DataClasses
{
    public class Grade : DataTableModel
    {
        public String GRADE { get; set; }

        public override string FieldsString
        {
            get { return DatabaseConstants.GRADES_TABLE_GRADE; }
        }

        public override string ValuesParamsString
        {
            get
            {
                return "@GRADE";
            }
        }

        public override List<Tuple<string, object>> ValuesList
        {
            get
            {
                return new List<Tuple<string, object>> { new Tuple<string, object>("GRADE", GRADE) };
            }
        }
    }
}
