using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 命名实体,无状态,不可更新
    /// </summary>
    /// <typeparam name="TID"></typeparam>
    public class NamingEntity<TID> : IDEntity<TID>
    {
        public string Name { set; get; }
    }
}
