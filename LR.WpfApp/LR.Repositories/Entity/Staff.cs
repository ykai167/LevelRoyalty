using LR.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 员工
    /// </summary>
    public partial class Staff : UpdateNamingEntity<Guid, Guid>
    {
        /// <summary>
        /// 员工号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdenNo { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobileNo { get; set; }

        /// <summary>
        /// 推荐人ID
        /// </summary>
        public Guid ReferrerID { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EntryTime { get; set; }
    }
}
