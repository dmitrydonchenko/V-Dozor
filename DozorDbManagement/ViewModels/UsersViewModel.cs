using DozorDatabaseLib;
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
    public class UsersViewModel : ViewModelBase, IPageViewModel
    {
        #region properties

        public string Name
        {
            get
            {
                return "Users Page";
            }
        }

        private UserModel currentUser;
        public UserModel CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                if (currentUser != value)
                {
                    currentUser = value;
                    RaisePropertyChanged("CurrentUser");
                }
            }
        }

        private ICommand _userDbRequestCommand;
        private ICommand _goToDefaultViewCommand;

        private DozorDatabase dozorDatabase;

        #endregion

        #region Constructor
        
        public UsersViewModel()
        {
            CurrentUser = new UserModel();
            dozorDatabase = DozorDatabase.Instance;
        }

        #endregion

        #region Commands

        public void UserDbRequest()
        {
            if (CurrentUser.Login == null || CurrentUser.Password == null || CurrentUser.PasswordConfirmation == null)
            {
                MessageBox.Show("Для добавления пользователя необходимо указать логин и пароль", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(CurrentUser.Password != CurrentUser.PasswordConfirmation)
            {
                MessageBox.Show("Введенные пароли не совпадают", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dozorDatabase.InsertUser(CurrentUser.Login, CurrentUser.Password, CurrentUser.Name))
            {
                MessageBox.Show("Пользователь был успешно добавлен", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Information);
                CurrentUser = new UserModel();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении пользователя", "Информация о запросе", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand UserDbRequestCommand
        {
            get
            {
                if (_userDbRequestCommand == null)
                {
                    _userDbRequestCommand = new RelayCommand(
                        param => UserDbRequest()
                    );
                }
                return _userDbRequestCommand;
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
