using DozorDbManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDbManagement.Models
{
    public class UserModel : ViewModelBase
    {
        private Int32 userId;
        public Int32 UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                base.RaisePropertyChanged("UserId");
            }
        }

        private String login;
        public String Login
        {
            get { return login; }
            set
            {
                login = value;
                base.RaisePropertyChanged("Login");
            }
        }

        private String name;
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                base.RaisePropertyChanged("Name");
            }
        }

        private String password;
        public String Password
        {
            get { return password; }
            set
            {
                password = value;
                base.RaisePropertyChanged("Password");
            }
        }

        private String passwordConfirmation;
        public String PasswordConfirmation
        {
            get { return passwordConfirmation; }
            set
            {
                passwordConfirmation = value;
                base.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        public UserModel(int userId, String login, String password, String passwordConfirmation, String name = null)
        {
            UserId = userId;
            Login = login;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
            Name = name;
        }

        public UserModel()
        {

        }
    }
}
