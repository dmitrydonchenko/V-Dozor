using DozorDatabaseLib;
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
    public class GradesViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Grade Page";
            }
        }

        private GradeModel currentGrade;
        public GradeModel CurrentGrade
        {
            get
            {
                return currentGrade;
            }
            set
            {
                if (currentGrade != value)
                {
                    currentGrade = value;
                    RaisePropertyChanged("CurrentGrade");
                }
            }
        }

        private ICommand _gradeDbRequestCommand;
        private ICommand _goToDefaultViewCommand;

        DozorDatabase dozorDatabase;

        #region Constructor

        public GradesViewModel()
        {
            CurrentGrade = new GradeModel();
            dozorDatabase = DozorDatabase.Instance;
        }

        #region Commands

        public void GradeDbRequest()
        {
            Grade grade = new Grade();
            grade.GRADE = CurrentGrade.Grade;
            if (dozorDatabase.InsertGrade(grade))
            {
                MessageBox.Show("Класс был успешно добавлен", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Information);
                CurrentGrade = new GradeModel();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении класса", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand GradeDbRequestCommand
        {
            get
            {
                if (_gradeDbRequestCommand == null)
                {
                    _gradeDbRequestCommand = new RelayCommand(
                        param => GradeDbRequest()
                    );
                }
                return _gradeDbRequestCommand;
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

        #endregion
    }
}
