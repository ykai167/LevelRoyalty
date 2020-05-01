using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 房间
    /// </summary>
    public class Room : UpdateNamingEntity<Guid, Guid>
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 房间分类ID
        /// </summary>
        public Guid CategoryID { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Summary { get; set; }
    }
}
