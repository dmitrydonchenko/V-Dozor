﻿using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using DozorDbManagement.Models;
using DozorUsbLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DozorDbManagement.ViewModels
{
    public class StudentsViewModel : ViewModelBase, IPageViewModel
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

        private ICommand _studentDbRequestCommand;
        private ICommand _goToDefaultViewCommand;

        private RfidReader rfidReader;
        private DozorDatabase dozorDatabase;

        #endregion

        #region Constructor

        public StudentsViewModel()
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
                if(dozorDatabase.GetStudentByRfid(rfid) != null)
                {
                    MessageBox.Show("Ученик с таким RFID уже существует", "Добавление ученика", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                SelectedStudent.Rfid = rfid;
            }
        }

        #endregion

        #region Commands

        public void StudentDbRequest()
        {
            Student student = new Student();
            student.FIRST_NAME = SelectedStudent.FirstName;
            student.MIDDLE_NAME = SelectedStudent.MiddleName;
            student.LAST_NAME = SelectedStudent.LastName;
            student.RFID = SelectedStudent.Rfid;
            student.GRADE_ID = SelectedStudent.GradeId;
            if(dozorDatabase.InsertStudent(student))
            {
                MessageBox.Show("Ученик был успешно добавлен", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении ученика", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand StudentDbRequestCommand
        {
            get
            {
                if (_studentDbRequestCommand == null)
                {
                    _studentDbRequestCommand = new RelayCommand(
                        param => StudentDbRequest()
                    );
                }
                return _studentDbRequestCommand;
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
