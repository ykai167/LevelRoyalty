using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    /// <summary>
    /// 管理员
    /// </summary>
    public class Admin : UpdateNamingEntity<Guid, Guid>
    {
        public string Password { get; set; }

        /// <summary>
        /// 超级管理员，普通管理员
        /// </summary>
        public int Type { get; set; }
    }
}
