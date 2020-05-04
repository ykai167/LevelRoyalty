using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR.Entity
{
    /// <summary>
    /// 结算批次
    /// </summary>
    public class SettleBatch : UpdateEntity<Guid, Guid>
    {
        /// <summary>
        /// 结算编号,yyyyMMdd
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 是否历史
        /// </summary>
        public bool IsHistory { get; set; }

        /// <summary>
        /// 开始时间:上一次结束即本次开始
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }    
}