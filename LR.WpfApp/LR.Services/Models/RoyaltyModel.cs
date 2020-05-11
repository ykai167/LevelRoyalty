using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    /// <summary>
    /// 提成明细
    /// </summary>
    public class _RoyaltyModel
    {
        /// <summary>
        /// 产生消费的员工ID
        /// </summary>
        public Guid StaffID { get; set; }
        /// <summary>
        /// 产生消费的员工
        /// </summary>
        public string StaffName { get; set; }

        /// <summary>
        /// 提成类型
        /// </summary>
        public Services.RoyaltyType RoyaltyType { get; set; }

        /// <summary>
        /// 消费时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 账期
        /// </summary>
        public int SettleNum { get; set; }

        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 提成百分比
        /// </summary>
        public decimal Percent { get; set; }

        /// <summary>
        /// 提成金额
        /// </summary>
        public decimal Royalty
        {
            get
            {
                return this.Amount * this.Percent;
            }
        }
    }
}
