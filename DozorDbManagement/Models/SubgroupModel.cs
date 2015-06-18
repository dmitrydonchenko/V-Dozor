using DozorDbManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozorDbManagement.Models
{
    public class SubgroupModel : ViewModelBase
    {
        private Int32 subgroupId;
        public Int32 SubgroupId
        {
            get { return subgroupId; }
            set
            {
                subgroupId = value;
                base.RaisePropertyChanged("SubgroupId");
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

        private String subgroup;
        public String Subgroup
        {
            get { return subgroup; }
            set
            {
                subgroup = value;
                base.RaisePropertyChanged("Subgroup");
            }
        }

        public SubgroupModel(Int32 gradeId, String subgroup)
        {
            GradeId = gradeId;
            Subgroup = subgroup;
        }

        public SubgroupModel()
        {
            GradeId = -1;
        }
    }
}
