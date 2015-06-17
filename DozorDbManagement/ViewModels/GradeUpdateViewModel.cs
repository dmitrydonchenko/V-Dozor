﻿using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using DozorDbManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DozorDbManagement.ViewModels
{
    public class GradeUpdateViewModel : ViewModelBase, IPageViewModel
    {
        #region properties

        public string Name
        {
            get
            {
                return "Grade Update Page";
            }
        }

        private GradeModel selectedGradeModel;
        public GradeModel SelectedGradeModel
        {
            get
            {
                return selectedGradeModel;
            }
            set
            {
                if (selectedGradeModel != value)
                {
                    selectedGradeModel = value;
                    RaisePropertyChanged("SelectedGradeModel");
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
                RaisePropertyChanged("Grades");
            }
        }

        private Int32 gradeId;
        public Int32 GradeId
        {
            get { return gradeId; }
            set
            {
                gradeId = value;
                selectedGrade = dozorDatabase.GetGradeById(gradeId);
                SelectedGradeModel = new GradeModel(gradeId, selectedGrade.GRADE);
                RaisePropertyChanged("GradeId");
            }
        }

        private ICommand _updateGradeCommand;
        private ICommand _goToDefaultViewCommand;
        private ICommand _deleteGradeCommand;

        private Grade selectedGrade;
        private DozorDatabase dozorDatabase;

        #endregion

        #region Constructor

        public GradeUpdateViewModel()
        {
            dozorDatabase = DozorDatabase.Instance;

            var gradesList = dozorDatabase.GetAllGrades();
            Grades = new List<GradeModel>();
            foreach (Grade grade in gradesList)
            {
                Grades.Add(new GradeModel(grade.ID, grade.GRADE));
                if(selectedGrade == null)
                {
                    selectedGrade = new Grade();
                    selectedGrade.ID = Grades.ElementAt(0).GradeId;
                    selectedGrade.GRADE = Grades.ElementAt(0).Grade;
                    SelectedGradeModel = Grades.ElementAt(0);
                }
            }
        }

        #endregion

        #region methods

        #endregion

        #region Commands

        private void UpdateGrade()
        {
            selectedGrade.GRADE = SelectedGradeModel.Grade;
            if (dozorDatabase.UpdateGrade(selectedGrade))
            {
                MessageBox.Show("Класс был успешно отредактирован", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Information);
                var gradesList = dozorDatabase.GetAllGrades();
                Grades = new List<GradeModel>();
                foreach (Grade grade in gradesList)
                {
                    Grades.Add(new GradeModel(grade.ID, grade.GRADE));
                    if (selectedGrade == null)
                    {
                        selectedGrade = new Grade();
                        selectedGrade.ID = Grades.ElementAt(0).GradeId;
                        selectedGrade.GRADE = Grades.ElementAt(0).Grade;
                        SelectedGradeModel = Grades.ElementAt(0);
                    }
                }
                RaisePropertyChanged("Grades");
            }
            else
            {
                MessageBox.Show("Ошибка при редактировании класса", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand UpdateGradeCommand
        {
            get
            {
                if (_updateGradeCommand == null)
                {
                    _updateGradeCommand = new RelayCommand(
                        param => UpdateGrade()
                    );
                }
                return _updateGradeCommand;
            }
        }

        private void DeleteGrade()
        {
            if (MessageBox.Show("Вы уверены?", "Удаления записи", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (selectedGrade != null)
                {
                    if (dozorDatabase.DeleteGradeById(selectedGrade.ID))
                    {
                        Grades.Remove(SelectedGradeModel);                        
                        if (Grades.Count > 0)
                        {
                            SelectedGradeModel = Grades.ElementAt(0);
                            selectedGrade = dozorDatabase.GetGradeById(SelectedGradeModel.GradeId);
                        }
                        else
                        {
                            SelectedGradeModel = null;
                            selectedGrade = null;
                        }
                        RaisePropertyChanged("Grades");
                        RaisePropertyChanged("GradeId");
                        MessageBox.Show("Запись успешно удалена", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении записи", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите класс для удаления", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public ICommand DeleteGradeCommand
        {
            get
            {
                if (_deleteGradeCommand == null)
                {
                    _deleteGradeCommand = new RelayCommand(
                        param => DeleteGrade()
                    );
                }
                return _deleteGradeCommand;
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
