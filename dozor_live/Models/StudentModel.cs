using dozor_live.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace dozor_live.Models
{
    class StudentModel : ViewModelBase
    {
        private String firstName;
        public String FirstName 
        {
            get { return firstName; }
            set 
            { 
                firstName = value;
                base.RaisePropertyChanged("FirstName"); 
            }
        }

        public String Greeting
        {
            get 
            {
                if (AttendanceIsIn)
                {
                    return "Добро пожаловать, " + FirstName + "                " +
                               "Вход в" + AttendanceDateTime.ToShortTimeString();
                }
                else
                {
                    return "До свидания, " + FirstName + "                " +
                               "Выход в " + AttendanceDateTime.ToShortTimeString();
                }
            }
        }        

        private List<MessageModel> messages;
        public List<MessageModel> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                if (messages != value)
                {
                    messages = value;
                    RaisePropertyChanged("Messages");
                }
            }
        }

        public DateTime AttendanceDateTime { get; set; }
        public Boolean AttendanceIsIn { get; set; }

        public StudentModel(String _firstName)
        {
            FirstName = _firstName;
        }
    }
}
