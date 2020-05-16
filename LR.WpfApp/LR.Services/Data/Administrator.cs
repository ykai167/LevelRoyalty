using LR.Tools;
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
        public Guid ID { get; set; }
        public string Name { get; set; }
        public AdminType Type { get; set; }

        internal static string PsString(string ps)
        {
            return $"{ps}{nameof(ps)}{ps}".ToMD5();
        }
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
