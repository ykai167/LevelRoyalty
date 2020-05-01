using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 日志:
    /// </summary>
    public class Log : IDEntity<Guid>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 旧数据
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string Old { get; set; }

        /// <summary>
        /// 新数据
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true)]
        public string New { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Summary { get; set; }        
    }
}
