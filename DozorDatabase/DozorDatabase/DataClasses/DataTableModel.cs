using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib.DataClasses
{
    public abstract class DataTableModel
    {
        public Int32 ID { get; set; }
        public abstract String FieldsString { get; }
        public abstract String ValuesParamsString { get; }
        public abstract List<Tuple<String, Object> > ValuesList { get; }
    }
}
