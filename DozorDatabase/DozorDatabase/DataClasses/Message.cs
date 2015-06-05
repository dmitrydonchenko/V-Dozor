using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib.DataClasses
{
    public class Message : DataTableModel
    {
        public Boolean IS_PERSONAL_MESSAGE { get; set; }
        public Int32 STUDENT_ID { get; set; }
        public Int32 GRADE_ID { get; set; }
        public String MESSAGE_TEXT { get; set; }
        public Byte[] MESSAGE_AUDIO { get; set; }
        public DateTime DATETIME { get; set; }
        public DateTime EXPIRATION_DATETIME { get; set; }
        public Int32 SENDER_ID { get; set; }

        public override string FieldsString
        {
            get 
            { 
                return DatabaseConstants.MESSAGES_TABLE_IS_PERSONAL_MESSAGE + "," +
                       DatabaseConstants.MESSAGES_TABLE_STUDENT_ID + "," +
                       DatabaseConstants.MESSAGES_TABLE_GRADE_ID + "," + 
                       DatabaseConstants.MESSAGES_TABLE_MESSAGE_TEXT + "," +
                       DatabaseConstants.MESSAGES_TABLE_MESSAGE_AUDIO + "," +
                       DatabaseConstants.MESSAGES_TABLE_DATETIME + "," +
                       DatabaseConstants.MESSAGES_TABLE_EXPIRATION_DATETIME + "," +
                       DatabaseConstants.MESSAGES_TABLE_SENDER_ID; 
            }
        }

        public override string ValuesParamsString
        {
            get
            {
                return "@IS_PERSONAL_MESSAGE," + 
                       "@STUDENT_ID," + 
                       "@GRADE_ID," + 
                       "@MESSAGE_TEXT," + 
                       "@MESSAGE_AUDIO," + 
                       "@DATETIME," + 
                       "@EXPIRATION_DATETIME," + 
                       "@SENDER_ID";
            }
        }

        public override List<Tuple<string, object>> ValuesList
        {
            get
            {
                return new List<Tuple<string, object>> { new Tuple<string, object>("IS_PERSONAL_MESSAGE", IS_PERSONAL_MESSAGE),
                                                         new Tuple<string, object>("STUDENT_ID", STUDENT_ID),
                                                         new Tuple<string, object>("GRADE_ID", GRADE_ID),
                                                         new Tuple<string, object>("MESSAGE_TEXT", MESSAGE_TEXT),
                                                         new Tuple<string, object>("MESSAGE_AUDIO", MESSAGE_AUDIO),
                                                         new Tuple<string, object>("DATETIME", DATETIME),
                                                         new Tuple<string, object>("EXPIRATION_DATETIME", EXPIRATION_DATETIME),
                                                         new Tuple<string, object>("SENDER_ID", SENDER_ID)};
            }
        }
    }
}
