using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Entity
{
    /// <summary>
    /// 奖励参数配置,记录各等级员工
    /// </summary>
    public class RoyaltyConfig : UpdateNamingEntity<Guid, Guid>
    {
        /// <summary>
        /// 提成类型,由系统定义枚举
        /// </summary>
        public int RoyaltyType { get; set; }

        /// <summary>
        /// 提成员工级别
        /// </summary>
        public Guid StaffLevel { get; set; }

        /// <summary>
        /// 被提成员工级别
        /// </summary>
        public Guid FromStaffLevel { get; set; }

        /// <summary>
        /// 提成比例
        /// </summary>
        public decimal Percent { get; set; }
    }
}