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
        public Byte[] PASSWORD { get; set; }
        public String NAME { get; set; }
        public Byte[] SALT { get; set; }

        public override string FieldsString
        {
            get { return DatabaseConstants.USERS_TABLE_LOGIN + "," +
                         DatabaseConstants.USERS_TABLE_PASSWORD + "," +
                         DatabaseConstants.USERS_TABLE_NAME + "," +
                         DatabaseConstants.USERS_TABLE_SALT; }
        }

        public override string ValuesParamsString
        {
            get
            {
                return "@LOGIN," +
                       "@PASSWORD," +
                       "@NAME" + 
                       "@SALT";
            }
        }

        public override List<Tuple<string, object>> ValuesList
        {
            get
            {
                return new List<Tuple<string, object>> { new Tuple<string, object>("LOGIN", LOGIN),
                                                           new Tuple<string, object>("PASSWORD", PASSWORD),
                                                           new Tuple<string, object>("NAME", NAME),
                                                           new Tuple<string, object>("SALT", SALT)};
            }
        }
    }
}
