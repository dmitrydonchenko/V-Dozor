using DozorDatabaseLib.DataClasses;
using DozorDbManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDbManagement.Models
{
    public class StudentModel : ViewModelBase
    {
        private String firstName;
        public String FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                FullName = FirstName + " " + MiddleName + " " + LastName;
                base.RaisePropertyChanged("FirstName");
            }
        }

        private String middleName;
        public String MiddleName
        {
            get { return middleName; }
            set
            {
                middleName = value;
                FullName = FirstName + " " + MiddleName + " " + LastName;
                base.RaisePropertyChanged("MiddleName");
            }
        }

        private String lastName;
        public String LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                FullName = FirstName + " " + MiddleName + " " + LastName;
                base.RaisePropertyChanged("LastName");
            }
        }

        private Int32 gradeId;
        public Int32 GradeId
        {
            get { return gradeId; }
            set
            {
                gradeId = value;
                base.RaisePropertyChanged("GradeId");
            }
        }

        private String rfid;
        public String Rfid
        {
            get { return rfid; }
            set
            {
                rfid = value;
                base.RaisePropertyChanged("Rfid");
            }
        }

        private String fullName;
        public String FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                base.RaisePropertyChanged("FullName");
            }
        }

        public StudentModel(String firstName, 
                            String middleName, 
                            String lastName,
                            Int32 gradeId,
                            String rfid)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            GradeId = gradeId;
            Rfid = rfid;
            FullName = FirstName + " " + MiddleName + " " + LastName;
        }

        public StudentModel()
        {
            FirstName = "";
            MiddleName = "";
            LastName = "";
            FullName = "";
        }
    }
}
