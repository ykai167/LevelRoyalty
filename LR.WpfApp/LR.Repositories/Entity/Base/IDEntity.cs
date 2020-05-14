using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    public class IDEntity<TID>
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        [Newtonsoft.Json.JsonIgnore]
        public TID ID { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public DateTime CreateDate { get; set; }
    }

    public class LogEntity<TID, TDataID, TOperatorID> : IDEntity<TID>
    {
        public TDataID DataID { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public TOperatorID OperatorID { get; set; }
        /// <summary>
        /// 数据操作类型
        /// </summary>
        public int Type { get; set; }
    }
}
