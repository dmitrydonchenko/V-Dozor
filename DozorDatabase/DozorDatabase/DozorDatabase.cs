using DozorDatabaseLib.DataClasses;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
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

        #region DozorDatabase public requests methods

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
        /// Get students by subgroup id
        /// </summary>
        /// <param name="subgroupId"></param>
        /// <returns></returns>
        public IEnumerable<Student> GetStudentsBySubgroup(Int32 subgroupId)
        {
            var studentsSubgroups = (IEnumerable<StudentSubgroup>)GetRecordsFromTable(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE, new string[] { DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_SUBGROUP_ID }, new string[] { subgroupId.ToString() });
            List<Student> students = new List<Student>();
            foreach(StudentSubgroup studentSubgroup in studentsSubgroups)
            {
                students.Add(GetStudentById(studentSubgroup.STUDENT_ID));
            }
            return (IEnumerable <Student>) students;
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
        /// Returns messages relevant to specified student
        /// </summary>
        /// <param name="_StudentId">Student Id</param>
        /// <param name="_GradeId">Grade Id</param>
        /// <returns>Collection of messages</returns>
        public IEnumerable<Message> GetStudentMessages(int _StudentId, int _GradeId)
        {
            IEnumerable<Message> messages = (IEnumerable<Message>) GetRecordsFromTable(DatabaseConstants.MESSAGES_TABLE,
                new string[] { DatabaseConstants.MESSAGES_TABLE_STUDENT_ID },
                new string[] { _StudentId.ToString() } );
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

        /// <summary>
        /// Returns subgroups by grade id
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public IEnumerable<Subgroup> GetSubgroupsByGradeId(int gradeId)
        {
            IEnumerable<Subgroup> subgroups = (IEnumerable<Subgroup>)GetRecordsFromTable(DatabaseConstants.SUBGROUP_TABLE,
                new string[] { DatabaseConstants.SUBGROUP_TABLE_GRADE_ID },
                new string[] { gradeId.ToString() });
            return subgroups;
        }

        /// <summary>
        /// Returns subgroup by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Subgroup GetSubgroupById(int id)
        {
            IEnumerable<Subgroup> subgroups = (IEnumerable<Subgroup>)GetRecordsFromTable(DatabaseConstants.SUBGROUP_TABLE,
                new string[] { DatabaseConstants.SUBGROUP_TABLE_ID },
                new string[] { id.ToString() });
            if(subgroups.Count() > 0)
            {
                return subgroups.ElementAt(0);
            }
            return null;
        }

        /// <summary>
        /// Checks whether student belongs to subgroup
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="subgroupId"></param>
        /// <returns></returns>
        public Boolean IsStudentInSubgroup(int studentId, int subgroupId)
        {
            IEnumerable < StudentSubgroup > studentSubgroup = (IEnumerable<StudentSubgroup>)GetRecordsFromTable(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE,
                new string[] { DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_STUDENT_ID, DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_SUBGROUP_ID },
                new string[] { studentId.ToString(), subgroupId.ToString() },
                new string[] { "AND" });
            if(studentSubgroup.Count() == 0)
            {
                return false;
            }
            return true;
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
        /// Insert subgroup to db
        /// </summary>
        /// <param name="subgroup"></param>
        /// <returns></returns>
        public Boolean InsertSubgroup(Subgroup subgroup)
        {
            return InsertRecordToDb(DatabaseConstants.SUBGROUP_TABLE, subgroup);
        }

        /// <summary>
        /// Insert student-subgroup record to db
        /// </summary>
        /// <param name="studentSubgroup"></param>
        /// <returns></returns>
        public Boolean InsertStudentSubgroup(StudentSubgroup studentSubgroup)
        {
            return InsertRecordToDb(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE, studentSubgroup);
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

        /// <summary>
        /// Delete student by id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public Boolean DeleteStudentById(int studentId)
        {
            return DeleteRecords(DatabaseConstants.STUDENTS_TABLE, new Tuple<string, object>(DatabaseConstants.STUDENTS_TABLE_ID, studentId));
        }

        /// <summary>
        /// Delete grade by id
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public Boolean DeleteGradeById(int gradeId)
        {
            return DeleteRecords(DatabaseConstants.GRADES_TABLE, new Tuple<string, object>(DatabaseConstants.GRADES_TABLE_ID, gradeId));
        }

        /// <summary>
        /// Delete subgroup by id
        /// </summary>
        /// <param name="subgroupId"></param>
        /// <returns></returns>
        public Boolean DeleteSubgroupById(int subgroupId)
        {
            return DeleteRecords(DatabaseConstants.SUBGROUP_TABLE, new Tuple<string, object>(DatabaseConstants.SUBGROUP_TABLE_ID, subgroupId));
        }

        /// <summary>
        /// Delete all subgroups with specified grade id
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public Boolean DeleteSubgroupByGradeId(int gradeId)
        {
            return DeleteRecords(DatabaseConstants.SUBGROUP_TABLE, new Tuple<string, object>(DatabaseConstants.SUBGROUP_TABLE_GRADE_ID, gradeId));
        }

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean DeleteUserById(int userId)
        {
            return DeleteRecords(DatabaseConstants.USERS_TABLE, new Tuple<string, object>(DatabaseConstants.USERS_TABLE, userId));
        }

        /// <summary>
        /// Delete students-subgroups by subgroup id
        /// </summary>
        /// <param name="subgroupId"></param>
        /// <returns></returns>
        public Boolean DeleteSubgroupStudentBySubgroupId(int subgroupId)
        {
            return DeleteRecords(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE, new Tuple<string, object>(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_SUBGROUP_ID, subgroupId));
        }

        /// <summary>
        /// Delete subgroups-students by grade id
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public Boolean DeleteSubgroupStudentByGradeId(int gradeId)
        {
            var subroups = GetSubgroupsByGradeId(gradeId);
            if(subroups != null)
            {
                foreach (Subgroup subgroup in subroups)
                {
                    if (!DeleteRecords(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE, new Tuple<string, object>(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_SUBGROUP_ID, subgroup.ID)))
                        return false;
                }
            }            
            return true;
        }

        /// <summary>
        /// Delete subgroups-students by student id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public Boolean DeleteSubgroupStudentByStudentId(int studentId)
        {
            return DeleteRecords(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE, new Tuple<string, object>(DatabaseConstants.STUDENTS_SUBGROUPS_TABLE_STUDENT_ID, studentId));
        }

        #endregion

        #endregion

        #region DozorDatabase private requests methods

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
                tableName != DatabaseConstants.USERS_TABLE &&
                tableName != DatabaseConstants.SUBGROUP_TABLE &&
                tableName != DatabaseConstants.STUDENTS_SUBGROUPS_TABLE))
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
                else if (tableName == DatabaseConstants.SUBGROUP_TABLE)
                {
                    tableCollection = connection.Query<Subgroup>(sqlQuery);
                }
                else if (tableName == DatabaseConstants.STUDENTS_SUBGROUPS_TABLE)
                {
                    tableCollection = connection.Query<StudentSubgroup>(sqlQuery);
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
                tableName != DatabaseConstants.USERS_TABLE &&
                tableName != DatabaseConstants.SUBGROUP_TABLE &&
                tableName != DatabaseConstants.STUDENTS_SUBGROUPS_TABLE))
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
                tableName != DatabaseConstants.USERS_TABLE &&
                tableName != DatabaseConstants.SUBGROUP_TABLE &&
                tableName != DatabaseConstants.STUDENTS_SUBGROUPS_TABLE))
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

        /// <summary>
        /// Delete records from a table by specified condition
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="value">Tuple that contains param's name and value</param>
        /// <returns></returns>
        private Boolean DeleteRecords(String tableName, Tuple<string, object> value)
        {
            if (tableName == null ||
               (tableName != DatabaseConstants.STUDENTS_TABLE &&
                tableName != DatabaseConstants.GRADES_TABLE &&
                tableName != DatabaseConstants.MESSAGES_TABLE &&
                tableName != DatabaseConstants.ATTENDANCE_TABLE &&
                tableName != DatabaseConstants.USERS_TABLE &&
                tableName != DatabaseConstants.SUBGROUP_TABLE &&
                tableName != DatabaseConstants.STUDENTS_SUBGROUPS_TABLE))
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

        #region Authentification methods

        private byte[] GenerateSalt()
        {
            int saltSize = 8;

            byte[] saltBytes = new byte[saltSize];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            rng.GetNonZeroBytes(saltBytes);

            return saltBytes;
        }

        private byte[] ComputeHash(string plainText, byte[] saltBytes = null)
        {
            if (saltBytes == null)
            {
                saltBytes = GenerateSalt();
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = new SHA1Managed();

            // Return computed hash value of our plain text with appended salt.
            return hash.ComputeHash(plainTextWithSaltBytes);
        }          

        #endregion

        #region Requests to users table

        /// <summary>
        /// Returns user id by login and password
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Int32 getUserIdByLoginAndPassword(String login, String password)
        {
            User user = getUserByLogin(login);
            if (user == null)
            {
                return -1;
            }
            byte[] saltBytes = user.SALT;
            byte[] passwordBytes = ComputeHash(password, saltBytes);
            if (passwordBytes == user.PASSWORD)
            {
                return user.ID;
            }
            return -1;
        }

        /// <summary>
        /// Returns user's login by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public String getUserLoginById(int userId)
        {
            User user = getUserById(userId);
            if (user != null)
            {
                return user.LOGIN;
            }
            return null;
        }

        /// <summary>
        /// Returns user's name by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public String getUserNameById(int userId)
        {
            User user = getUserById(userId);
            if (user != null)
            {
                return user.NAME;
            }
            return null;
        }

        private User getUserByLogin(String login)
        {
            IEnumerable<User> users = (IEnumerable<User>)GetRecordsFromTable(DatabaseConstants.USERS_TABLE,
                new string[] { DatabaseConstants.USERS_TABLE_LOGIN },
                new string[] { login.ToString() });
            if (users.Count() > 0)
            {
                return users.ElementAt(0);
            }
            return null;
        }

        private User getUserById(int userId)
        {
            IEnumerable<User> users = (IEnumerable<User>)GetRecordsFromTable(DatabaseConstants.USERS_TABLE,
                new string[] { DatabaseConstants.USERS_TABLE_ID },
                new string[] { userId.ToString() });
            if (users.Count() > 0)
            {
                return users.ElementAt(0);
            }
            return null;
        }

        /// <summary>
        /// Insert user in db
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Boolean InsertUser(String login, String password, String name)
        {
            User user = new User();
            user.LOGIN = login;
            user.SALT = GenerateSalt();
            user.PASSWORD = ComputeHash(password, user.SALT);
            user.NAME = name;
            return InsertRecordToDb(DatabaseConstants.USERS_TABLE, user);
        } 

        #endregion
    }
}
