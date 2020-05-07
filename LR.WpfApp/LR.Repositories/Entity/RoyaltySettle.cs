using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Entity
{
    /// <summary>
    /// 提成支出,账期结算时生成
    /// </summary>
    public class RoyaltySettle : UpdateEntity<Guid, Guid>
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid StaffID { get; set; }

        /// <summary>
        /// 结算账期
        /// </summary>
        public int SettleNum { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 是否已支出
        /// </summary>
        public bool IsExpend { get; set; }

        /// <summary>
        /// 是否本人领取
        /// </summary>
        public bool IsSelf { get; set; }

        /// <summary>
        /// 实际领取人
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Receiver { get; set; }
    }
}