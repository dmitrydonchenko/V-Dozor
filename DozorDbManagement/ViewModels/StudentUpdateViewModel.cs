using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using DozorDbManagement.Models;
using DozorUsbLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DozorDbManagement.ViewModels
{
    class StudentUpdateViewModel : ViewModelBase, IPageViewModel
    {
        #region properties

        public string Name
        {
            get
            {
                return "Student Page";
            }
        }

        private StudentModel currentStudent;
        public StudentModel SelectedStudent
        {
            get
            {
                return currentStudent;
            }
            set
            {
                if (currentStudent != value)
                {
                    currentStudent = value;
                    RaisePropertyChanged("SelectedStudent");
                }
            }
        }

        private List<GradeModel> grades;
        public List<GradeModel> Grades
        {
            get { return grades; }
            set
            {
                grades = value;
            }
        }

        private List<StudentModel> students;
        public List<StudentModel> Students
        {
            get { return students; }
            set
            {
                students = value;
                RaisePropertyChanged("Students");
            }
        }

        private Int32 gradeId;
        public Int32 GradeId
        {
            get { return gradeId; }
            set
            {
                if (gradeId == value)
                    return;
                gradeId = value;
                SelectedStudent.GradeId = gradeId;
                Students.Clear();
                var studentsList = dozorDatabase.GetStudentsByGrade(gradeId);
                List<StudentModel> studentsTempList = new List<StudentModel>();
                foreach (Student student in studentsList)
                {
                    studentsTempList.Add(new StudentModel(student.FIRST_NAME, student.MIDDLE_NAME, student.LAST_NAME, student.GRADE_ID, student.RFID));
                }
                Students = studentsTempList;
                if (Students.Count > 0)
                    SelectedStudent = Students.ElementAt(0);
            }
        }

        private ICommand _selectStudentCommand;
        private ICommand _updateStudentCommand;
        private ICommand _goToDefaultViewCommand;

        private RfidReader rfidReader;
        private DozorDatabase dozorDatabase;

        #endregion

        #region Constructor

        public StudentUpdateViewModel()
        {
            SelectedStudent = new StudentModel();
            // Get grades list from db
            dozorDatabase = DozorDatabase.Instance;
            var gradesList = dozorDatabase.GetAllGrades();
            Grades = new List<GradeModel>();
            foreach(Grade grade in gradesList)
            {
                Grades.Add(new GradeModel(grade.ID, grade.GRADE));
            }

            Students = new List<StudentModel>();
            if(gradesList.Count() > 0)
            {
                var studentsList = dozorDatabase.GetStudentsByGrade(Grades.ElementAt(0).GradeId);                
                foreach (Student student in studentsList)
                {
                    Students.Add(new StudentModel(student.FIRST_NAME, student.MIDDLE_NAME, student.LAST_NAME, student.GRADE_ID, student.RFID));                    
                }
                if (Students.Count > 0)
                    SelectedStudent = Students.ElementAt(0);
            }                        

            //Start Usb Service
            rfidReader = RfidReader.Instance;
            rfidReader.Rfid_Updated += new EventHandler<string>(RfidReceived);
        }

        #endregion

        #region methods

        private void RfidReceived(object sender, String rfid)
        {
            if (SelectedStudent.Rfid != rfid)
            {
                Student student = dozorDatabase.GetStudentByRfid(rfid);
                SelectedStudent.Rfid = rfid;
                if (student != null)
                {                    
                    SelectedStudent.FirstName = student.FIRST_NAME;
                    SelectedStudent.MiddleName = student.MIDDLE_NAME;
                    SelectedStudent.LastName = student.LAST_NAME;
                    SelectedStudent.GradeId = student.GRADE_ID;
                }                              
            }
        }

        #endregion

        #region Commands

        public void SelectStudent()
        {
            String rfid = SelectedStudent.Rfid;
            Student student = dozorDatabase.GetStudentByRfid(rfid);
            SelectedStudent.FirstName = student.FIRST_NAME;
            SelectedStudent.MiddleName = student.MIDDLE_NAME;
            SelectedStudent.LastName = student.LAST_NAME;
            SelectedStudent.GradeId = student.GRADE_ID;
        }

        public ICommand SelectStudentCommand
        {
            get
            {
                if (_selectStudentCommand == null)
                {
                    _selectStudentCommand = new RelayCommand(
                        param => SelectStudent()
                    );
                }
                return _selectStudentCommand;
            }
        }

        private void UpdateStudent()
        {

        }

        public ICommand UpdateStudentCommand
        {
            get
            {
                if (_updateStudentCommand == null)
                {
                    _updateStudentCommand = new RelayCommand(
                        param => UpdateStudent()
                    );
                }
                return _updateStudentCommand;
            }
        }

        private void GoToDefaultView()
        {
            DefaultViewModel dvm = new DefaultViewModel();
            ApplicationViewModel.AppContext.CurrentPageViewModel = dvm;
        }

        public ICommand GoToDefaultViewCommand
        {
            get
            {
                if (_goToDefaultViewCommand == null)
                {
                    _goToDefaultViewCommand = new RelayCommand(
                        param => GoToDefaultView()
                    );
                }
                return _goToDefaultViewCommand;
            }
        }

        #endregion
    }
}
