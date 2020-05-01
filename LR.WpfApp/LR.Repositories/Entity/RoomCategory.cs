using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 房间类别
    /// </summary>
    public class RoomCategory : UpdateNamingEntity<Guid, Guid>
    {
        /// <summary>
        /// 最低消费
        /// </summary>
        public decimal MinCharge { get; set; }
    }
}
