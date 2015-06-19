using DozorDatabaseLib;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DozorDbManagement.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        #region Fields

        private ICommand _changePageCommand;

        private ICommand _addStudentCommand;
        private ICommand _updateStudentCommand;

        private ICommand _addGradeCommand;
        private ICommand _updateGradeCommand;

        private ICommand _addSubgroupCommand;
        private ICommand _updateSubgroupCommand;

        private ICommand _addUserCommand;
        private ICommand _updateUserCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private static ApplicationViewModel instance;

        #endregion

        public static ApplicationViewModel AppContext
        {
            get
            {
                if(instance == null)
                {
                    instance = new ApplicationViewModel();
                }
                return instance;
            }
        }

        private ApplicationViewModel()
        {
            // Create Database instance "D:\\Votum\\DozorDatabase\\Students.fdbB;"
            FbConnectionStringBuilder connectString = new FbConnectionStringBuilder();
            connectString.Database = "D:\\Votum\\DozorDatabase\\Students.fdb";
            connectString.Dialect = 3;
            connectString.UserID = "SYSDBA";
            connectString.Password = "masterkey";
            connectString.Charset = "win1251";
            DozorDatabase.CreateInstance(connectString.ConnectionString);

            // Add available pages
            PageViewModels.Add(new DefaultViewModel());

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        #region Properties / Commands

        private void GoToStudentsView()
        {
            StudentsViewModel svm = new StudentsViewModel();
            AppContext.CurrentPageViewModel = svm;
        }

        private void GoToStudentUpdateView()
        {
            StudentUpdateViewModel suvm = new StudentUpdateViewModel();
            AppContext.CurrentPageViewModel = suvm;
        }

        private void GoToGradesView()
        {
            GradesViewModel gvm = new GradesViewModel();
            AppContext.CurrentPageViewModel = gvm;
        }

        private void GoToGradeUpdateView()
        {
            GradeUpdateViewModel guvm = new GradeUpdateViewModel();
            AppContext.CurrentPageViewModel = guvm;
        }

        private void GoToSubgroupsView()
        {
            SubgroupsViewModel svm = new SubgroupsViewModel();
            AppContext.CurrentPageViewModel = svm;
        }

        private void GoToSubgroupUpdateView()
        {
            SubgroupUpdateViewModel suvm = new SubgroupUpdateViewModel();
            AppContext.CurrentPageViewModel = suvm;
        }

        private void GoToUsersView()
        {
            UsersViewModel uvm = new UsersViewModel();
            AppContext.CurrentPageViewModel = uvm;
        }

        private void GoToUserUpdateView()
        {
            SubgroupUpdateViewModel suvm = new SubgroupUpdateViewModel();
            AppContext.CurrentPageViewModel = suvm;
        }

        public ICommand AddStudentCommand
        {
            get
            {
                if (_addStudentCommand == null)
                {
                    _addStudentCommand = new RelayCommand(
                        param => GoToStudentsView()
                    );
                }
                return _addStudentCommand;
            }
        }

        public ICommand UpdateStudentCommand
        {
            get
            {
                if (_updateStudentCommand == null)
                {
                    _updateStudentCommand = new RelayCommand(
                        param => GoToStudentUpdateView()
                    );
                }
                return _updateStudentCommand;
            }
        }

        public ICommand AddGradeCommand
        {
            get
            {
                if (_addGradeCommand == null)
                {
                    _addGradeCommand = new RelayCommand(
                        param => GoToGradesView()
                    );
                }
                return _addGradeCommand;
            }
        }

        public ICommand UpdateGradeCommand
        {
            get
            {
                if (_updateGradeCommand == null)
                {
                    _updateGradeCommand = new RelayCommand(
                        param => GoToGradeUpdateView()
                    );
                }
                return _updateGradeCommand;
            }
        }

        public ICommand AddSubgroupCommand
        {
            get
            {
                if (_addSubgroupCommand == null)
                {
                    _addSubgroupCommand = new RelayCommand(
                        param => GoToSubgroupsView()
                    );
                }
                return _addSubgroupCommand;
            }
        }

        public ICommand UpdateSubgroupCommand
        {
            get
            {
                if (_updateSubgroupCommand == null)
                {
                    _updateSubgroupCommand = new RelayCommand(
                        param => GoToSubgroupUpdateView()
                    );
                }
                return _updateSubgroupCommand;
            }
        }

        public ICommand AddUserCommand
        {
            get
            {
                if (_addUserCommand == null)
                {
                    _addUserCommand = new RelayCommand(
                        param => GoToUsersView()
                    );
                }
                return _addUserCommand;
            }
        }

        public ICommand UpdateUserCommand
        {
            get
            {
                if (_updateUserCommand == null)
                {
                    _updateUserCommand = new RelayCommand(
                        param => GoToUserUpdateView()
                    );
                }
                return _updateUserCommand;
            }
        }     

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    if (!PageViewModels.Contains(value))
                        PageViewModels.Add(value);

                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }

        #endregion

        #region Methods

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }        

        #endregion
    }
}
