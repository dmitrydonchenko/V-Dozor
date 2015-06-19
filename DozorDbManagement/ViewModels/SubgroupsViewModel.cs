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
    public class SubgroupsViewModel : ViewModelBase, IPageViewModel
    {
        #region properties

        public string Name
        {
            get
            {
                return "Subgroup Page";
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

        private SubgroupModel currentSubgroup;
        public SubgroupModel CurrentSubgroup
        {
            get
            {
                return currentSubgroup;
            }
            set
            {
                if (currentSubgroup != value)
                {
                    currentSubgroup = value;
                    RaisePropertyChanged("CurrentSubgroup");
                }
            }
        }

        private ICommand _subgroupDbRequestCommand;
        private ICommand _goToDefaultViewCommand;

        DozorDatabase dozorDatabase;

        #endregion

        #region Constructor

        public SubgroupsViewModel()
        {
            CurrentSubgroup = new SubgroupModel();
            dozorDatabase = DozorDatabase.Instance;

            // Get grades list from db
            dozorDatabase = DozorDatabase.Instance;
            var gradesList = dozorDatabase.GetAllGrades();
            Grades = new List<GradeModel>();
            CurrentSubgroup.GradeId = -1;
            foreach (Grade grade in gradesList)
            {
                Grades.Add(new GradeModel(grade.ID, grade.GRADE));
                if (CurrentSubgroup.GradeId == -1)
                    CurrentSubgroup.GradeId = Grades.ElementAt(0).GradeId;
            }
        }

        #endregion

        #region Commands

        public void SubgroupDbRequest()
        {
            if (CurrentSubgroup.GradeId == -1)
            {
                MessageBox.Show("Для добавления подгруппы необходимо класс", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentSubgroup.Subgroup == null)
            {
                MessageBox.Show("Для добавления подгруппы необходимо указать её название", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Subgroup subgroup = new Subgroup();
            subgroup.GRADE_ID = CurrentSubgroup.GradeId;
            subgroup.SUBGROUP = CurrentSubgroup.Subgroup;
            if (dozorDatabase.InsertSubgroup(subgroup))
            {
                MessageBox.Show("Подгруппа была успешно добавлена", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Information);
                CurrentSubgroup = new SubgroupModel();
                CurrentSubgroup.GradeId = subgroup.GRADE_ID;
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении подгруппы", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SubgroupDbRequestCommand
        {
            get
            {
                if (_subgroupDbRequestCommand == null)
                {
                    _subgroupDbRequestCommand = new RelayCommand(
                        param => SubgroupDbRequest()
                    );
                }
                return _subgroupDbRequestCommand;
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
