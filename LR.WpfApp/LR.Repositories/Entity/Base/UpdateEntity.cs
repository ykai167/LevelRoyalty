using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 可更新实体,有状态,有操作人
    /// </summary>
    public class UpdateEntity<TID, TOperatorID> : IDEntity<TID>
    {
        /// <summary>
        /// 200 正常，400 删除，600 禁用
        /// </summary>
        public int State { set; get; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public TOperatorID OperatorID { get; set; }
    }

    public class DataState
    {
        /// <summary>
        /// 正常
        /// </summary>
        public const int Normal = 200;
        /// <summary>
        /// 删除
        /// </summary>
        public const int Delete = 400;
        /// <summary>
        /// 禁用
        /// </summary>
        public const int Disable = 600;
    }
}
