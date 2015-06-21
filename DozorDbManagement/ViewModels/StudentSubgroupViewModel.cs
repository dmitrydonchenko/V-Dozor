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
    public class StudentSubgroupViewModel : ViewModelBase, IPageViewModel
    {
        #region properties

        public string Name
        {
            get
            {
                return "Student Subgroup Page";
            }
        }

        private SubgroupModel selectedSubgroupModel;
        public SubgroupModel SelectedSubgroupModel
        {
            get
            {
                return selectedSubgroupModel;
            }
            set
            {
                if (selectedSubgroupModel != value)
                {
                    selectedSubgroupModel = value;
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

        private ObservableCollection<StudentModel> gradeStudents;
        public ObservableCollection<StudentModel> GradeStudents
        {
            get { return gradeStudents; }
            set
            {
                gradeStudents = value;
                RaisePropertyChanged("Students");
            }
        }

        private ObservableCollection<StudentModel> subgroupStudents;
        public ObservableCollection<StudentModel> SubgroupStudents
        {
            get { return subgroupStudents; }
            set
            {
                subgroupStudents = value;
                RaisePropertyChanged("Students");
            }
        }

        private Int32 gradeId;
        public Int32 GradeId
        {
            get { return gradeId; }
            set
            {
                gradeId = value;
                // update subgroups list
                Subgroups.Clear();
                var subgroupsList = dozorDatabase.GetSubgroupsByGradeId(gradeId);
                if (subgroupsList == null)
                    return;
                foreach (Subgroup subgroup in subgroupsList)
                {
                    Subgroups.Add(new SubgroupModel(subgroup.ID, subgroup.GRADE_ID, subgroup.SUBGROUP));
                }
                if (Subgroups.Count > 0)
                {
                    SelectedSubgroupModel = Subgroups.ElementAt(0);
                    SelectedSubgroupId = SelectedSubgroupModel.SubgroupId;
                }
                else
                {
                    SelectedSubgroupModel = null;
                    SelectedSubgroupModel = null;
                    SelectedSubgroupId = -1;
                }
                // update grade students list
                GradeStudents.Clear();
                var studentsList = dozorDatabase.GetStudentsByGrade(gradeId);
                foreach (Student student in studentsList)
                {
                    GradeStudents.Add(new StudentModel(student.FIRST_NAME, student.MIDDLE_NAME, student.LAST_NAME, student.GRADE_ID, student.RFID));
                }
                if (GradeStudents.Count > 0)
                {
                    SelectedStudentGradeModel = GradeStudents.ElementAt(0);
                    CurrentStudentGradeRfid = SelectedStudentGradeModel.Rfid;
                }
                else
                {
                    SelectedStudentGradeModel = null;
                    CurrentStudentGradeRfid = null;
                }
                RaisePropertyChanged("GradeId");
            }
        }

        private Int32 selectedSubgroupId;
        public Int32 SelectedSubgroupId
        {
            get { return selectedSubgroupId; }
            set
            {
                selectedSubgroupId = value;
                // update subgroup students list
                SubgroupStudents.Clear();
                var studentsList = dozorDatabase.GetStudentsBySubgroup(selectedSubgroupId);
                foreach (Student student in studentsList)
                {
                    SubgroupStudents.Add(new StudentModel(student.FIRST_NAME, student.MIDDLE_NAME, student.LAST_NAME, student.GRADE_ID, student.RFID));
                }
                if (SubgroupStudents.Count > 0)
                {
                    SelectedStudentSubgroupModel = SubgroupStudents.ElementAt(0);
                    CurrentStudentSubgroupRfid = SelectedStudentSubgroupModel.Rfid;
                }
                else
                {
                    SelectedStudentSubgroupModel = null;
                    CurrentStudentSubgroupRfid = null;
                }
                RaisePropertyChanged("SelectedSubgroupId");
            }
        }

        private StudentModel selectedStudentGradeModel;
        public StudentModel SelectedStudentGradeModel
        {
            get { return selectedStudentGradeModel; }
            set
            {
                selectedStudentGradeModel = value;
                if(value != null)
                    CurrentStudentGradeRfid = selectedStudentGradeModel.Rfid;
            }
        }

        private StudentModel selectedStudentSubgroupModel;
        public StudentModel SelectedStudentSubgroupModel
        {
            get { return selectedStudentSubgroupModel; }
            set
            {
                selectedStudentSubgroupModel = value;
                if (value != null)
                    CurrentStudentSubgroupRfid = selectedStudentSubgroupModel.Rfid;
            }
        }

        private String currentStudentGradeRfid;
        public String CurrentStudentGradeRfid
        {
            get { return currentStudentGradeRfid; }
            set
            {
                currentStudentGradeRfid = value;
            }
        }

        private String currentStudentSubgroupRfid;
        public String CurrentStudentSubgroupRfid
        {
            get { return currentStudentSubgroupRfid; }
            set
            {
                currentStudentSubgroupRfid = value;
            }
        }

        private ICommand _addStudentToSubgroupCommand;
        private ICommand _deleteStudentFromSubgroupCommand;

        private DozorDatabase dozorDatabase;

        #endregion

        #region constructor

        public StudentSubgroupViewModel()
        {
            SelectedSubgroupModel = new SubgroupModel();
            Subgroups = new ObservableCollection<SubgroupModel>();
            GradeStudents = new ObservableCollection<StudentModel>();
            SubgroupStudents = new ObservableCollection<StudentModel>();
            // Get grades list from db
            dozorDatabase = DozorDatabase.Instance;
            var gradesList = dozorDatabase.GetAllGrades();
            Grades = new ObservableCollection<GradeModel>();
            foreach (Grade grade in gradesList)
            {
                Grades.Add(new GradeModel(grade.ID, grade.GRADE));
            }

            SelectedSubgroupId = -1;
            if (gradesList.Count() > 0)
            {
                GradeId = gradesList.ElementAt(0).ID;
                if (Subgroups.Count > 0)
                {
                    SelectedSubgroupModel = Subgroups.ElementAt(0);
                    SelectedSubgroupId = SelectedSubgroupModel.SubgroupId;
                }
                else
                {
                    var subgroupsList = dozorDatabase.GetSubgroupsByGradeId(Grades.ElementAt(0).GradeId);
                    if (subgroupsList == null)
                        return;
                    foreach (Subgroup subgroup in subgroupsList)
                    {
                        Subgroups.Add(new SubgroupModel(subgroup.ID, subgroup.GRADE_ID, subgroup.SUBGROUP));
                    }
                }
                if (GradeStudents.Count > 0)
                {
                    SelectedStudentGradeModel = GradeStudents.ElementAt(0);
                    CurrentStudentGradeRfid = SelectedStudentGradeModel.Rfid;
                }
                else
                {
                    var studentsGradeList = dozorDatabase.GetStudentsByGrade(Grades.ElementAt(0).GradeId);
                    foreach (Student student in studentsGradeList)
                    {
                        GradeStudents.Add(new StudentModel(student.FIRST_NAME, student.MIDDLE_NAME, student.LAST_NAME, student.GRADE_ID, student.RFID));
                    }
                }
                if (SubgroupStudents.Count > 0)
                {
                    SelectedStudentSubgroupModel = SubgroupStudents.ElementAt(0);
                    CurrentStudentSubgroupRfid = SelectedStudentSubgroupModel.Rfid;
                }
                else
                {
                    var studentsSubgroupList = dozorDatabase.GetStudentsBySubgroup(Subgroups.ElementAt(0).SubgroupId);
                    foreach (Student student in studentsSubgroupList)
                    {
                        SubgroupStudents.Add(new StudentModel(student.FIRST_NAME, student.MIDDLE_NAME, student.LAST_NAME, student.GRADE_ID, student.RFID));
                    }
                }
            }
        }

        #endregion

        #region Commands

        public void AddStudentToSubgroup()
        {
            if (CurrentStudentGradeRfid == null)
            {
                MessageBox.Show("Выберите ученика для добавления в подгруппу", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            StudentSubgroup studentSubgroup = new StudentSubgroup();
            studentSubgroup.STUDENT_ID = dozorDatabase.GetStudentByRfid(CurrentStudentGradeRfid).ID;
            studentSubgroup.SUBGROUP_ID = SelectedSubgroupId;
            if(dozorDatabase.IsStudentInSubgroup(studentSubgroup.STUDENT_ID, studentSubgroup.SUBGROUP_ID))
            {
                MessageBox.Show("Ученик уже состоит в данной подгруппе", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dozorDatabase.InsertStudentSubgroup(studentSubgroup))
            {
                MessageBox.Show("Ученик был успешно добавлен", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedSubgroupId = SelectedSubgroupId;
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении ученика", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand AddStudentToSubgroupCommand
        {
            get
            {
                if (_addStudentToSubgroupCommand == null)
                {
                    _addStudentToSubgroupCommand = new RelayCommand(
                        param => AddStudentToSubgroup()
                    );
                }
                return _addStudentToSubgroupCommand;
            }
        }

        private void DeleteStudentFromSubgroup()
        {
            if (MessageBox.Show("Вы уверены?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (SelectedStudentSubgroupModel != null)
                {
                    var student = dozorDatabase.GetStudentByRfid(SelectedStudentSubgroupModel.Rfid);
                    if(student == null)
                    {
                        MessageBox.Show("Ошибка при удалении записи", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (dozorDatabase.DeleteSubgroupStudentByStudentId(student.ID))
                    {
                        foreach (StudentModel studentModel in SubgroupStudents)
                        {
                            if (studentModel.Rfid == student.RFID)
                            {
                                SubgroupStudents.Remove(studentModel);
                                break;
                            }
                        }
                        RaisePropertyChanged("SubgroupStudents");
                        CurrentStudentSubgroupRfid = null;
                        if (SubgroupStudents.Count > 0)
                        {
                            SelectedStudentSubgroupModel = SubgroupStudents.ElementAt(0);
                            CurrentStudentSubgroupRfid = SubgroupStudents.ElementAt(0).Rfid;
                        }
                        else
                        {
                            SelectedStudentSubgroupModel = null;
                            CurrentStudentSubgroupRfid = null;
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

        public ICommand DeleteStudentFromSubgroupCommand
        {
            get
            {
                if (_deleteStudentFromSubgroupCommand == null)
                {
                    _deleteStudentFromSubgroupCommand = new RelayCommand(
                        param => DeleteStudentFromSubgroup()
                    );
                }
                return _deleteStudentFromSubgroupCommand;
            }
        }

        #endregion
    }
}
