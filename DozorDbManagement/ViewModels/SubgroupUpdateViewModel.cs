using DozorDatabaseLib;
using DozorDatabaseLib.DataClasses;
using DozorDbManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DozorDbManagement.ViewModels
{
    public class SubgroupUpdateViewModel : ViewModelBase, IPageViewModel
    {
        #region properties

        public string Name
        {
            get
            {
                return "Subgroup Page";
            }
        }

        private SubgroupModel currentSubgroupModel;
        public SubgroupModel SelectedSubgroupModel
        {
            get
            {
                return currentSubgroupModel;
            }
            set
            {
                if (currentSubgroupModel != value)
                {
                    currentSubgroupModel = value;
                    RaisePropertyChanged("SelectedSubgroupModel");
                }
            }
        }

        private ObservableCollection<GradeModel> grades;
        public ObservableCollection<GradeModel> Grades
        {
            get { return grades; }
            set
            {
                grades = value;
            }
        }

        private ObservableCollection<SubgroupModel> subgroups;
        public ObservableCollection<SubgroupModel> Subgroups
        {
            get { return subgroups; }
            set
            {
                subgroups = value;
                RaisePropertyChanged("Subgroups");
            }
        }

        private Int32 gradeId;
        public Int32 GradeId
        {
            get { return gradeId; }
            set
            {
                gradeId = value;
                Subgroups.Clear();
                var subgroupsList = dozorDatabase.GetSubgroupsByGradeId(gradeId);
                if (subgroupsList == null)
                    return;
                foreach (Subgroup subgroup in subgroupsList)
                {
                    Subgroups.Add(new SubgroupModel(subgroup.GRADE_ID, subgroup.SUBGROUP));
                }
                if (Subgroups.Count > 0)
                {
                    SelectedSubgroupModel = Subgroups.ElementAt(0);
                    CurrentSubgroupId = SelectedSubgroupModel.SubgroupId;
                }
                else
                {
                    SelectedSubgroupModel = null;
                    selectedSubgroup = null;
                    SelectedSubgroupModel = null;
                }
            }
        }

        private Int32 currentSubgroupId;
        public Int32 CurrentSubgroupId
        {
            get { return currentSubgroupId; }
            set
            {
                currentSubgroupId = value;
                SelectSubgroup();
            }
        }

        private ICommand _updateSubgroupCommand;
        private ICommand _goToDefaultViewCommand;
        private ICommand _deleteSubgroupCommand;

        private DozorDatabase dozorDatabase;

        private Subgroup selectedSubgroup;

        #endregion

        #region constructor

        public SubgroupUpdateViewModel()
        {
            SelectedSubgroupModel = new SubgroupModel();
            // Get grades list from db
            dozorDatabase = DozorDatabase.Instance;
            var gradesList = dozorDatabase.GetAllGrades();
            Grades = new ObservableCollection<GradeModel>();
            foreach (Grade grade in gradesList)
            {
                Grades.Add(new GradeModel(grade.ID, grade.GRADE));
            }

            Subgroups = new ObservableCollection<SubgroupModel>();
            if (gradesList.Count() > 0)
            {
                GradeId = gradesList.ElementAt(0).ID;
                if (Subgroups.Count > 0)
                {
                    SelectedSubgroupModel = Subgroups.ElementAt(0);
                    CurrentSubgroupId = SelectedSubgroupModel.SubgroupId;
                }
                else
                {
                    var subgroupsList = dozorDatabase.GetSubgroupsByGradeId(Grades.ElementAt(0).GradeId);
                    if (subgroupsList == null)
                        return;
                    foreach (Subgroup subgroup in subgroupsList)
                    {
                        Subgroups.Add(new SubgroupModel(subgroup.GRADE_ID, subgroup.SUBGROUP));
                    }
                }
            }                        
        }

        #endregion

        #region methods

        private void SelectSubgroup()
        {
            if (CurrentSubgroupId == null)
                return;
            selectedSubgroup = dozorDatabase.GetSubgroupById(CurrentSubgroupId);
            if (selectedSubgroup != null)
            {
                SelectedSubgroupModel = new SubgroupModel(selectedSubgroup.GRADE_ID,
                                                          selectedSubgroup.SUBGROUP);
            }
        }

        #endregion

        #region Commands

        private void UpdateSubgroup()
        {

            selectedSubgroup.GRADE_ID = SelectedSubgroupModel.GradeId;
            selectedSubgroup.SUBGROUP = SelectedSubgroupModel.Subgroup;
            if (dozorDatabase.UpdateSubgroup(selectedSubgroup))
            {
                MessageBox.Show("Подгруппа была успешно отредактирована", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Information);
                GradeId = GradeId;
            }
            else
            {
                MessageBox.Show("Ошибка при редактировании подгруппы", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand UpdateSubgroupCommand
        {
            get
            {
                if (_updateSubgroupCommand == null)
                {
                    _updateSubgroupCommand = new RelayCommand(
                        param => UpdateSubgroup()
                    );
                }
                return _updateSubgroupCommand;
            }
        }

        private void DeleteSubgroup()
        {
            if (MessageBox.Show("Вы уверены?", "Удаления записи", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (selectedSubgroup != null)
                {
                    if (dozorDatabase.DeleteSubgroupById(selectedSubgroup.ID))
                    {
                        foreach (SubgroupModel SubgroupModel in Subgroups)
                        {
                            if (SubgroupModel.SubgroupId == selectedSubgroup.ID)
                            {
                                Subgroups.Remove(SubgroupModel);
                                break;
                            }
                        }
                        RaisePropertyChanged("Subgroups");
                        CurrentSubgroupId = -1;
                        if (Subgroups.Count > 0)
                        {
                            SelectedSubgroupModel = Subgroups.ElementAt(0);
                            CurrentSubgroupId = Subgroups.ElementAt(0).SubgroupId;
                            selectedSubgroup = dozorDatabase.GetSubgroupById(CurrentSubgroupId);
                        }
                        else
                        {
                            SelectedSubgroupModel = null;
                            selectedSubgroup = null;
                        }
                        MessageBox.Show("Запись успешно удалена", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении записи", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите ученика для удаления", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public ICommand DeleteSubgroupCommand
        {
            get
            {
                if (_deleteSubgroupCommand == null)
                {
                    _deleteSubgroupCommand = new RelayCommand(
                        param => DeleteSubgroup()
                    );
                }
                return _deleteSubgroupCommand;
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
