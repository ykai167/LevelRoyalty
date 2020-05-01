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
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public TOperatorID OperatorID { get; set; }
    }
}
