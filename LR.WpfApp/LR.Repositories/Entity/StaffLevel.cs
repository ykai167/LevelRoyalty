using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 员工等级
    /// </summary>
    public class StaffLevel : UpdateNamingEntity<Guid, Guid>
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 升级条件，达到最小数量升到上一级
        /// </summary>
        public int MinCount { get; set; }
    }
}
