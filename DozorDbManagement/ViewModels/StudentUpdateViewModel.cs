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

        private StudentModel currentStudentModel;
        public StudentModel SelectedStudentModel
        {
            get
            {
                return currentStudentModel;
            }
            set
            {
                if (currentStudentModel != value)
                {
                    currentStudentModel = value;
                    RaisePropertyChanged("SelectedStudentModel");
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
                Students.Clear();
                var studentsList = dozorDatabase.GetStudentsByGrade(gradeId);
                List<StudentModel> studentsTempList = new List<StudentModel>();
                foreach (Student student in studentsList)
                {
                    studentsTempList.Add(new StudentModel(student.FIRST_NAME, student.MIDDLE_NAME, student.LAST_NAME, student.GRADE_ID, student.RFID));
                }
                Students = studentsTempList;
                if (Students.Count > 0)
                {
                    SelectedStudentModel = Students.ElementAt(0);
                    CurrentRfid = SelectedStudentModel.Rfid;
                }
            }
        }

        private String currentRfid;
        public String CurrentRfid
        {
            get { return currentRfid; }
            set
            {
                currentRfid = value;
                SelectStudent();
            }
        }

        private ICommand _updateStudentCommand;
        private ICommand _goToDefaultViewCommand;
        private ICommand _deleteStudentCommand;

        private RfidReader rfidReader;
        private DozorDatabase dozorDatabase;

        private Student selectedStudent;

        #endregion

        #region Constructor

        public StudentUpdateViewModel()
        {
            SelectedStudentModel = new StudentModel();
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
                {
                    SelectedStudentModel = Students.ElementAt(0);
                    CurrentRfid = SelectedStudentModel.Rfid;
                }
            }                        

            //Start Usb Service
            rfidReader = RfidReader.Instance;
            rfidReader.Rfid_Updated += new EventHandler<string>(RfidReceived);
        }

        #endregion

        #region methods

        private void RfidReceived(object sender, String rfid)
        {
            if (CurrentRfid != rfid)
            {
                CurrentRfid = rfid;
                SelectStudent();                          
            }
        }

        private void SelectStudent()
        {
            if (currentRfid == null)
                return;
            selectedStudent = dozorDatabase.GetStudentByRfid(currentRfid);
            if (selectedStudent != null)
            {
                SelectedStudentModel = new StudentModel(selectedStudent.FIRST_NAME,
                                                                  selectedStudent.MIDDLE_NAME,
                                                                  selectedStudent.LAST_NAME,
                                                                  selectedStudent.GRADE_ID,
                                                                  currentRfid);
            }
        }

        #endregion

        #region Commands       

        private void UpdateStudent()
        {
            selectedStudent.FIRST_NAME = SelectedStudentModel.FirstName;
            selectedStudent.MIDDLE_NAME = SelectedStudentModel.MiddleName;
            selectedStudent.LAST_NAME = SelectedStudentModel.LastName;
            selectedStudent.GRADE_ID = SelectedStudentModel.GradeId;
            selectedStudent.RFID = SelectedStudentModel.Rfid;
            if(dozorDatabase.UpdateStudent(selectedStudent))
            {
                MessageBox.Show("Ученик был успешно отредактирован", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка при редактировании ученика", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void DeleteStudent()
        {

        }

        public ICommand DeleteStudentCommand
        {
            get
            {
                if (_deleteStudentCommand == null)
                {
                    _deleteStudentCommand = new RelayCommand(
                        param => DeleteStudent()
                    );
                }
                return _deleteStudentCommand;
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
