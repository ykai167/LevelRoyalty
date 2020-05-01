using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 工作组成员
    /// </summary>
    public class WorkGroupMember : UpdateEntity<Guid, Guid>
    {
        /// <summary>
        /// 工作组ID
        /// </summary>
        public Guid WorkGroupID { set; get; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid StaffID { get; set; }
        /// <summary>
        /// 人员类别:管理人员按照WorkGroupManagerCategory记ID,普通人员默认值
        /// </summary>
        public Guid CategoryID { get; set; }
    }
}
