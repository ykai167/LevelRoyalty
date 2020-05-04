using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    public class WorkGroupMemberModel : StaffModel
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid StaffID { get; set; }

        /// <summary>
        /// 工作组ID
        /// </summary>
        public Guid GroupID { get; set; }
        public WorkGroupManagerCategoryModel Category { get; set; }
    }
    public class WorkGroups
    {
        IEnumerable<WorkGroupModel> data;
        public WorkGroups()
        {

        }
    }

    public class WorkGroupModel
    {
        public string Name { get; set; }
        public StaffModel[] Members { get; set; }
        public StaffModel[] Managers { get; set; }
        public bool IsMember(StaffModel staff)
        {
            return this.Managers.Any(p => p.ID == staff.ID);
        }
    }
}
