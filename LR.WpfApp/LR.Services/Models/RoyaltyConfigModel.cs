using LR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    /* /// <summary>
        [EnumName(Name = "订房奖励")]
        Reservation,

        [EnumName(Name = "管理奖励")]
        Administration,

        [EnumName(Name = "协作奖励")]
        Cooperation,

        [EnumName(Name = "超越奖励")]
        Transcend,

        [EnumName(Name = "工作组奖励占比")]
        WorkGroupRoyalty,

        [EnumName(Name = "工作组管理奖励")]
        WorkGroup*/
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

    public class ReservationRoyalty
    {
        public decimal Percent { get; set; }

    }
}
