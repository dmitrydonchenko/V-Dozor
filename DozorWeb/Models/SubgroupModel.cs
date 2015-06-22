using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DozorWeb.Models
{
    public class SubgroupModel
    {
        public Int32 SubgroupId { get; set; }
        public String Subgroup { get; set; }

        public SubgroupModel(int subgroupId, String subgroup)
        {
            SubgroupId = subgroupId;
            Subgroup = subgroup;
        }
    }
}