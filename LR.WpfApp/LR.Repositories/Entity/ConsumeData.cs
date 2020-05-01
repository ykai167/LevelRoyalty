using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Entity
{
    /// <summary>
    /// 消费数据记录
    /// </summary>
    public class ConsumeData : UpdateEntity<Guid, Guid>
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid StaffID { get; set; }

        /// <summary>
        /// 房间ID
        /// </summary>
        public Guid RoomID { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}