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
    public class Log : LogEntity<Guid, Guid, Guid>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 新数据
        /// </summary>
        [SqlSugar.SugarColumn(IsNullable = true, ColumnDataType = "TEXT")]
        public string Data { get; set; }
    }
}
