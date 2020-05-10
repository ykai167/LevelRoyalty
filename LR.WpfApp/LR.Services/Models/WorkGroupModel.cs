using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    public class WorkGroupMemberModel : StaffBase
    {
        /// <summary>
        /// 工作组ID
        /// </summary>
        public Guid GroupID { get; set; }

        public WorkGroupManagerCategoryModel Category { get; set; }
    }
}
