using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    public class RoyaltyStatisticsPageModel
    {
    }

    public class RoyaltyStatisticsModel
    {
        /// <summary>
        /// 提成员工ID
        /// </summary>
        public Guid StaffID { get; set; }

        /// <summary>
        /// 提成员工
        /// </summary>
        public string StaffName { get; set; }

        /// <summary>
        /// 账期
        /// </summary>
        public int SettleNum { get; set; }

        /// <summary>
        /// 总计金额
        /// </summary>
        public decimal Total { get; set; }
    }
}
