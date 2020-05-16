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
        /// <summary>
        /// 登录名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 超级管理员，普通管理员
        /// </summary>
        public int Type { get; set; }
    }
}
