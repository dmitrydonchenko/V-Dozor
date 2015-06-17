using DozorDatabaseLib.DataClasses;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DozorDatabaseLib
{
    public class DozorDatabase
    {
        #region DozorDatabase members

        /// <summary> Database singleton instance </summary>
        private static DozorDatabase instance;

        /// <summary> Connection to database </summary>
        private FbConnection connection;

        public static DozorDatabase Instance { get { return instance; } }

        #endregion

        #region DozorDatabase constructor

        private DozorDatabase(String connectionString)
        {
            if (connection == null)
            {
                connection = new FbConnection(connectionString);
            }
        }

        #endregion

        #region DozorDatabase public methods

        /// <summary>
        /// Creates an instance of database if none already exists
        /// </summary>
        /// <param name="connectionString"> Connection string </param>
        public static void CreateInstance(String connectionString)
        {
            if (instance == null)
            {
                instance = new DozorDatabase(connectionString);
            }
        }

        #region Select methods

        /// <summary>
        /// Returns student by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Student GetStudentById(int id)
        {
            var students = GetRecordsFromTable(DatabaseConstants.STUDENTS_TABLE, new string[] { DatabaseConstants.STUDENTS_TABLE_ID }, new string[] { id.ToString() });
            Student student = null;
            if (students != null)
            {
                student = students.ElementAt(0);
            }
            return student;
        }

        /// <summary>
        /// Returns student by RFID
        /// </summary>
        /// <param name="rfid">Student's RFID</param>
        /// <returns></returns>
        public Student GetStudentByRfid(String rfid)
        {
            var students = GetRecordsFromTable(DatabaseConstants.STUDENTS_TABLE, new string[] { DatabaseConstants.STUDENTS_TABLE_RFID }, new string[] { rfid });
            Student student = null;
            if (students.Count() > 0)
            {
                student = students.ElementAt(0);                               
            }
            return student;
        }

        /// <summary>
        /// Returns students by grade
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public IEnumerable<Student> GetStudentsByGrade(Int32 gradeId)
        {
            return (IEnumerable<Student>)GetRecordsFromTable(DatabaseConstants.STUDENTS_TABLE, new string[] { DatabaseConstants.STUDENTS_TABLE_GRADE_ID }, new string[] { gradeId.ToString() });
        }

        /// <summary>
        /// Returns Grade by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Grade GetGradeById(int id)
        {
            var grades = GetRecordsFromTable(DatabaseConstants.GRADES_TABLE, new string[] { DatabaseConstants.GRADES_TABLE_ID }, new string[] { id.ToString() });
            Grade grade = null;
            if (grades != null)
            {
                grade = grades.ElementAt(0);
            }
            return grade;
        }

        /// <summary>
        /// Returns all grades
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Grade> GetAllGrades()
        {
            return (IEnumerable<Grade>) GetRecordsFromTable(DatabaseConstants.GRADES_TABLE);
        }

        /// <summary>
        /// Returns User by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(int id)
        {
            var users = GetRecordsFromTable(DatabaseConstants.USERS_TABLE, new string[] { DatabaseConstants.USERS_TABLE_ID }, new string[] { id.ToString() });
            User user = null;
            if (users != null)
            {
                user = users.ElementAt(0);
            }
            return user;
        }

        /// <summary>
        /// Returns messages relevant to specified student
        /// </summary>
        /// <param name="_StudentId">Student Id</param>
        /// <param name="_GradeId">Grade Id</param>
        /// <returns>Collection of messages</returns>
        public IEnumerable<Message> GetStudentMessages(int _StudentId, int _GradeId)
        {
            IEnumerable<Message> messages = (IEnumerable<Message>) GetRecordsFromTable(DatabaseConstants.MESSAGES_TABLE,
                new string[] { DatabaseConstants.MESSAGES_TABLE_STUDENT_ID, DatabaseConstants.MESSAGES_TABLE_GRADE_ID },
                new string[] { _StudentId.ToString(), _GradeId.ToString() }, new string [] { "OR" });
            return messages;  
        }

        /// <summary>
        /// Returns attendancies relevant to specified student
        /// </summary>
        /// <param name="_StudentId"></param>
        /// <returns></returns>
        public IEnumerable<Attendance> GetStudentAttendancies(int studentId)
        {
            IEnumerable<Attendance> attendancies = (IEnumerable<Attendance>) GetRecordsFromTable(DatabaseConstants.ATTENDANCE_TABLE,
                new string[] { DatabaseConstants.ATTENDANCE_TABLE_STUDENT_ID },
                new string[] { studentId.ToString() });
            return attendancies;  
        }
        
        /// <summary>
        /// Returns attendancies by date relevant to specified student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public IEnumerable<Attendance> GetStudentAttendanciesByDate(int studentId, DateTime dateTime)
        {
            IEnumerable<Attendance> allAttendancies = (IEnumerable<Attendance>)GetRecordsFromTable(DatabaseConstants.ATTENDANCE_TABLE,
                new string[] { DatabaseConstants.ATTENDANCE_TABLE_STUDENT_ID },
                new string[] { studentId.ToString() });
            List<Attendance> attendanciesByDate = new List<Attendance>();
            foreach(Attendance attendance in allAttendancies)
            {
                if(attendance.DATETIME.Year == dateTime.Year && attendance.DATETIME.Month == dateTime.Month && attendance.DATETIME.Day == dateTime.Day)
                {
                    attendanciesByDate.Add(attendance);
                }
            }
            return attendanciesByDate;
        }

        #endregion

        #region Insert methods

        /// <summary>
        /// Insert attendance to db
        /// </summary>
        /// <param name="attendance"></param>
        /// <returns></returns>
        public Boolean InsertAttendance(Attendance attendance)
        {
            return InsertRecordToDb(DatabaseConstants.ATTENDANCE_TABLE, attendance);
        }

        /// <summary>
        /// Insert grade to db
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public Boolean InsertGrade(Grade grade)
        {
            return InsertRecordToDb(DatabaseConstants.GRADES_TABLE, grade);
        }

        /// <summary>
        /// insert message to db
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Boolean InsertMessage(Message message)
        {
            return InsertRecordToDb(DatabaseConstants.MESSAGES_TABLE, message);
        }

        /// <summary>
        /// Insert student to db
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public Boolean InsertStudent(Student student)
        {
            return InsertRecordToDb(DatabaseConstants.STUDENTS_TABLE, student);
        }

        /// <summary>
        /// Insert user to db
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Boolean InsertUser(User user)
        {
            return InsertRecordToDb(DatabaseConstants.USERS_TABLE, user);
        }

        #endregion 

        #region Update methods

        public Boolean UpdateStudent(Student student)
        {
            return UpdateRecord(DatabaseConstants.STUDENTS_TABLE, student);
        }

        public Boolean UpdateGrade(Grade grade)
        {
            return UpdateRecord(DatabaseConstants.GRADES_TABLE, grade);
        }

        public Boolean UpdateSubgroup(Subgroup subgroup)
        {
            return UpdateRecord(DatabaseConstants.SUBGROUP_TABLE, subgroup);
        }

        public Boolean UpdateUser(User user)
        {
            return UpdateRecord(DatabaseConstants.USERS_TABLE, user);
        }

        #endregion

        #region Delete methods

        public Boolean DeleteStudentById(int studentId)
        {
            return DeleteRecords(DatabaseConstants.STUDENTS_TABLE, new Tuple<string, object>(DatabaseConstants.STUDENTS_TABLE_ID, studentId));
        }

        public Boolean DeleteGradeById(int gradeId)
        {
            return DeleteRecords(DatabaseConstants.GRADES_TABLE, new Tuple<string, object>(DatabaseConstants.GRADES_TABLE_ID, gradeId));
        }

        public Boolean DeleteSubgroupById(int subgroupId)
        {
            return DeleteRecords(DatabaseConstants.SUBGROUP_TABLE, new Tuple<string, object>(DatabaseConstants.SUBGROUP_TABLE, subgroupId));
        }

        public Boolean DeleteUserById(int userId)
        {
            return DeleteRecords(DatabaseConstants.USERS_TABLE, new Tuple<string, object>(DatabaseConstants.USERS_TABLE, userId));
        }

        public Boolean DeleteSubgroupStudentBySubgroupId(int subgroupId)
        {
            return DeleteRecords(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE, new Tuple<string, object>(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_SUBGROUP_ID, subgroupId));
        }

        public Boolean DeleteSubgroupStudentByStudentId(int studentId)
        {
            return DeleteRecords(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE, new Tuple<string, object>(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_STUDENT_ID, studentId));
        }

        #endregion

        #endregion

        #region DozorDatabase private methods

        /// <summary>
        /// Returns collection of records from database
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="fieldsList">List of fields in WHERE statement</param>
        /// <param name="valuesList">List of fields values in WHERE statement</param>
        /// <returns>Collection of records from database</returns>
        private IEnumerable<dynamic> GetRecordsFromTable(String tableName, String[] fieldsList = null, String[] valuesList = null, String[] operations = null)
        {
            if (tableName == null ||
                (tableName != DatabaseConstants.STUDENTS_TABLE &&
                tableName != DatabaseConstants.GRADES_TABLE &&
                tableName != DatabaseConstants.MESSAGES_TABLE &&
                tableName != DatabaseConstants.ATTENDANCE_TABLE &&
                tableName != DatabaseConstants.USERS_TABLE))
                return null;
            String sqlQuery = "SELECT * FROM " + tableName;
            int fieldsCount = 0;
            if (fieldsList != null && valuesList != null)
            {
                sqlQuery += " WHERE ";
                if (fieldsList.Count() != valuesList.Count())
                    return null;
                if (operations != null)
                {
                    if (fieldsList.Count() != operations.Count() + 1)
                        return null;
                }
                fieldsCount = fieldsList.Count();
            }
            for (int i = 0; i < fieldsCount; i++)
            {
                sqlQuery += fieldsList.ElementAt(i) + "='" + valuesList.ElementAt(i) + "'";
                if (i < fieldsList.Count() - 1)
                    sqlQuery += " " + operations[i] + " ";
            }
            IEnumerable<dynamic> tableCollection = null;
            try
            {
                connection.Open();
                if (tableName == DatabaseConstants.STUDENTS_TABLE)
                {
                    tableCollection = connection.Query<Student>(sqlQuery);
                }
                else if (tableName == DatabaseConstants.GRADES_TABLE)
                {
                    tableCollection = connection.Query<Grade>(sqlQuery);
                }
                else if (tableName == DatabaseConstants.MESSAGES_TABLE)
                {
                    tableCollection = connection.Query<Message>(sqlQuery);
                }
                else if (tableName == DatabaseConstants.USERS_TABLE)
                {
                    tableCollection = connection.Query<User>(sqlQuery);
                }
                else if (tableName == DatabaseConstants.ATTENDANCE_TABLE)
                {
                    tableCollection = connection.Query<Attendance>(sqlQuery);
                }
                connection.Close();
            }
            catch (Exception e)
            {
                return null;
            }
            return tableCollection;
        }

        /// <summary>
        /// Insert rec
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dataTableModel"></param>
        /// <returns></returns>
        private Boolean InsertRecordToDb(String tableName, DataTableModel dataTableModel)
        {
            if (tableName == null ||
                (tableName != DatabaseConstants.STUDENTS_TABLE &&
                tableName != DatabaseConstants.GRADES_TABLE &&
                tableName != DatabaseConstants.MESSAGES_TABLE &&
                tableName != DatabaseConstants.ATTENDANCE_TABLE &&
                tableName != DatabaseConstants.USERS_TABLE))
                return false;
            if (dataTableModel == null)
                return false;
            String sqlQuery = "INSERT INTO " + tableName + " (" +
                              dataTableModel.FieldsString + ")" + "VALUES (" +
                              dataTableModel.ValuesParamsString + ")";
            // values
            try
            {
                connection.Open();
            }
            catch(Exception e)
            {
                return false;
            }
            FbCommand fbCom = new FbCommand(sqlQuery, connection);
            foreach(Tuple<string, object> valueTuple in dataTableModel.ValuesList)
            {
                fbCom.Parameters.AddWithValue(valueTuple.Item1, valueTuple.Item2);
            }
            try
            {
                fbCom.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception e)
            {
                connection.Close();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Update query
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dataTableModel"></param>
        /// <returns></returns>
        private Boolean UpdateRecord(String tableName, DataTableModel dataTableModel)
        {
            if (tableName == null ||
                (tableName != DatabaseConstants.STUDENTS_TABLE &&
                tableName != DatabaseConstants.GRADES_TABLE &&
                tableName != DatabaseConstants.MESSAGES_TABLE &&
                tableName != DatabaseConstants.ATTENDANCE_TABLE &&
                tableName != DatabaseConstants.USERS_TABLE))
                return false;
            String fieldsString = "ID," + dataTableModel.FieldsString;
            List<Tuple<string, object>> valuesList = dataTableModel.ValuesList;
            valuesList.Insert(0, new Tuple<string, object>("ID", dataTableModel.ID));
            String valuesParamsString = "@ID," + dataTableModel.ValuesParamsString;

            String sqlQuery = "UPDATE OR INSERT INTO " + tableName + " (" +
                              fieldsString + ") " + " VALUES (" +
                              valuesParamsString + ")";
            // values
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                return false;
            }
            FbCommand fbCom = new FbCommand(sqlQuery, connection);
            foreach (Tuple<string, object> valueTuple in valuesList)
            {
                fbCom.Parameters.AddWithValue(valueTuple.Item1, valueTuple.Item2);
            }
            try
            {
                int res = fbCom.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                return false;
            }
            return true;
        }

        private Boolean DeleteRecords(String tableName, Tuple<string, object> value)
        {
            if (tableName == null ||
               (tableName != DatabaseConstants.STUDENTS_TABLE &&
               tableName != DatabaseConstants.GRADES_TABLE &&
               tableName != DatabaseConstants.MESSAGES_TABLE &&
               tableName != DatabaseConstants.ATTENDANCE_TABLE &&
               tableName != DatabaseConstants.USERS_TABLE))
                return false;
            String sqlQuery = "DELETE FROM " + tableName + " WHERE " +
                              value.Item1 + "=" + "@" + value.Item1;
            // values
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                return false;
            }
            FbCommand fbCom = new FbCommand(sqlQuery, connection);
            fbCom.Parameters.AddWithValue(value.Item1, value.Item2);
            try
            {
                int res = fbCom.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                return false;
            }
            return true;
        }

        #endregion
    }
}
