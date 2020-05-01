using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public class Administrator
    {
        public static Administrator Current { get; set; }
        public string Name { get; set; }
        public AdminType Type { get; set; }
    }

    public enum AdminType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        Super,
        /// <summary>
        /// 普通管理员
        /// </summary>
        Ordin
    }
}
