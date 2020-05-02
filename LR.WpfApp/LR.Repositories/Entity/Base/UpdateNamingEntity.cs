using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 可更新命名实体
    /// </summary>
    /// <typeparam name="TID"></typeparam>
    /// <typeparam name="TOperatorID"></typeparam>
    public class UpdateNamingEntity<TID, TOperatorID> : UpdateEntity<TID, TOperatorID>
    {
        public string Name { set; get; }
    }
}
