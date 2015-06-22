using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDatabaseLib
{
    public class DatabaseConstants
    {
        #region Tables names

        public static String STUDENTS_TABLE = "STUDENTS_TABLE";
        public static String GRADES_TABLE = "GRADES_TABLE";
        public static String MESSAGES_TABLE = "MESSAGES_TABLE";
        public static String ATTENDANCE_TABLE = "ATTENDANCE_TABLE";
        public static String USERS_TABLE = "USERS_TABLE";
        public static String SUBGROUP_TABLE = "SUBGROUP_TABLE";
        public static String STUDENTS_SUBGROUPS_TABLE = "STUDENTS_SUBGROUPS_TABLE";

        #endregion

        #region Students table fields

        public static String STUDENTS_TABLE_ID = "ID";
        public static String STUDENTS_TABLE_FIRST_NAME = "FIRST_NAME";
        public static String STUDENTS_TABLE_MIDDLE_NAME = "MIDDLE_NAME";
        public static String STUDENTS_TABLE_LAST_NAME = "LAST_NAME";
        public static String STUDENTS_TABLE_GRADE_ID = "GRADE_ID";
        public static String STUDENTS_TABLE_RFID = "RFID";

        #endregion

        #region Grades table fields

        public static String GRADES_TABLE_ID = "ID";
        public static String GRADES_TABLE_GRADE = "GRADE";

        #endregion

        #region Messages table fields

        public static String MESSAGES_TABLE_ID = "ID";
        public static String MESSAGES_TABLE_IS_PERSONAL_MESSAGE = "IS_PERSONAL_MESSAGE";
        public static String MESSAGES_TABLE_STUDENT_ID = "STUDENT_ID";
        public static String MESSAGES_TABLE_MESSAGE_TEXT = "MESSAGE_TEXT";
        public static String MESSAGES_TABLE_MESSAGE_AUDIO = "MESSAGE_AUDIO";
        public static String MESSAGES_TABLE_DATETIME = "DATETIME";
        public static String MESSAGES_TABLE_EXPIRATION_DATETIME = "EXPIRATION_DATETIME";
        public static String MESSAGES_TABLE_SENDER_ID = "SENDER_ID";

        #endregion

        #region Attendance table fields

        public static String ATTENDANCE_TABLE_ID = "ID";
        public static String ATTENDANCE_TABLE_DATETIME = "DATETIME";
        public static String ATTENDANCE_TABLE_STUDENT_ID = "STUDENT_ID";
        public static String ATTENDANCE_TABLE_SNAPSHOT = "SNAPSHOT";
        public static String ATTENDANCE_TABLE_IS_IN = "IS_IN";

        #endregion

        #region Users table fields

        public static String USERS_TABLE_ID = "ID";
        public static String USERS_TABLE_LOGIN = "LOGIN";
        public static String USERS_TABLE_PASSWORD = "PASSWORD";
        public static String USERS_TABLE_NAME = "NAME";
        public static String USERS_TABLE_SALT = "SALT";

        #endregion

        #region Subgroups table fields

        public static String SUBGROUP_TABLE_ID = "ID";
        public static String SUBGROUP_TABLE_GRADE_ID = "GRADE_ID";
        public static String SUBGROUP_TABLE_SUBGROUP = "SUBGROUP";

        #endregion

        #region Students_subgroups table fields

        public static String STUDENTS_SUBGROUPS_TABLE_ID = "ID";
        public static String STUDENTS_SUBGROUPS_TABLE_STUDENT_ID = "STUDENT_ID";
        public static String STUDENTS_SUBGROUPS_TABLE_SUBGROUP_ID = "SUBGROUP_ID";

        #endregion
    }
}
