using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 工作组管理员类别
    /// </summary>
    public class WorkGroupManagerCategory : UpdateNamingEntity<Guid, Guid>
    {
        /// <summary>
        /// 管理提成比例
        /// </summary>
        public decimal Percent { get; set; }
    }
}
