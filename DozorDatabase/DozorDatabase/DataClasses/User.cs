using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib.DataClasses
{
    public class User : DataTableModel
    {
        public String LOGIN { get; set; }
        public String PASSWORD { get; set; }
        public String NAME { get; set; }


        public override string FieldsString
        {
            get { return DatabaseConstants.USERS_TABLE_LOGIN + "," +
                         DatabaseConstants.USERS_TABLE_PASSWORD + "," +
                         DatabaseConstants.USERS_TABLE_NAME; }
        }

        public override string ValuesParamsString
        {
            get
            {
                return "@LOGIN," +
                       "@PASSWORD," +
                       "@NAME";
            }
        }

        public override List<Tuple<string, object>> ValuesList
        {
            get
            {
                return new List<Tuple<string, object>> { new Tuple<string, object>("LOGIN", LOGIN),
                                                           new Tuple<string, object>("PASSWORD", PASSWORD),
                                                           new Tuple<string, object>("NAME", NAME)};
            }
        }
    }
}
