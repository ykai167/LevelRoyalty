using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Entity
{
    /// <summary>
    /// 提成数据,及时记录
    /// </summary>
    public class Royalty : UpdateEntity<Guid, Guid>
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid StaffID { get; set; }

        /// <summary>
        /// 消费记录ID
        /// </summary>
        public Guid ConsumeDataID { get; set; }

        /// <summary>
        /// 提成类型,程序定义的提成类型枚举
        /// </summary>
        public int RoyaltyType { get; set; }

        /// <summary>
        /// 提成百分比,读取RoyaltyConfig中配置的比例
        /// </summary>
        [SqlSugar.SugarColumn(ColumnDataType = "MONEY")]
        public decimal Percent { get; set; }

        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettleNum { get; set; }
    }
}