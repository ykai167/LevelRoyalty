using LR.Entity;
using LR.Services;
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

    public class RoyaltyStatisticsModel : RoyaltySettle
    {
        public Guid ID { get; set; }
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

        public KeyValuePair<RoyaltyType, decimal>[] Items { get; set; }

        /// <summary>
        /// 总计金额
        /// </summary>
        public decimal Total
        {
            get
            {
                return Items.Sum(p => p.Value);
            }
        }
    }
}
