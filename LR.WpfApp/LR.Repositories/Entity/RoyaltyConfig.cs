using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Entity
{
    /// <summary>
    /// 奖励参数配置,记录各等级员工
    /// </summary>
    public class RoyaltyConfig : UpdateEntity<Guid, Guid>
    {
        /// <summary>
        /// 提成类型,由系统定义枚举
        /// </summary>
        public int RoyaltyType { get; set; }

        /// <summary>
        /// 提成者身份ID:员工级别ID或管理人员类别ID
        /// /// </summary>
        public Guid AcceptID { get; set; }

        /// <summary>
        /// 贡献提成者身份ID:员工级别ID,管理提成与几别无关
        /// </summary>
        public Guid ExpendID { get; set; }

        /// <summary>
        /// 提成比例
        /// </summary>
        public decimal Percent { get; set; }
    }
}