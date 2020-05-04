using LR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    public class RoyaltyConfigModel
    {
        public Services.RoyaltyType RoyaltyType { get; set; }

        /// <summary>
        /// 提成者身份ID:员工级别ID或管理人员类别ID
        /// /// </summary>
        public Guid AcceptID { get; set; }

        /// <summary>
        /// 贡献提成者身份ID:员工级别ID,管理提成与几别无关
        /// </summary>
        public Guid ExpendID { get; set; }

        /// <summary>
        /// 提成比例
        /// </summary>
        public decimal Percent { get; set; }
    }
}
