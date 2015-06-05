using DozorDbManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDbManagement.Models
{
    public class GradeModel : ViewModelBase
    {
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

        private String grade;
        public String Grade
        {
            get { return grade; }
            set
            {
                grade = value;
                base.RaisePropertyChanged("Grade");
            }
        }

        public GradeModel(Int32 gradeId, String grade)
        {
            GradeId = gradeId;
            Grade = grade;
        }
    }
}
