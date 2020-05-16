using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public enum LogType
    {
        /// <summary>
        /// 新增
        /// </summary>
        [EnumName(Name = "新增")]
        Insert,

        /// <summary>
        /// 更新
        /// </summary>
        [EnumName(Name = "更新")]
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        [EnumName(Name = "删除")]
        Delete,
        /// <summary>
        /// 登录
        /// </summary>
        [EnumName(Name = "登录")]
        Login
    }

    public enum RoyaltyType
    {
        /// <summary>
        /// 订房奖励
        /// </summary>
        [EnumName(Name = "订房奖励")]
        Reservation,

        /// <summary>
        /// 管理奖励
        /// </summary>
        [EnumName(Name = "管理奖励")]
        Administration,

        /// <summary>
        /// 协作奖励
        /// </summary>
        [EnumName(Name = "协作奖励")]
        Cooperation,

        /// <summary>
        /// 超越奖励
        /// </summary>
        [EnumName(Name = "超越奖励")]
        Transcend,
        /// <summary>
        /// 工作组奖励占比
        /// </summary>
        [EnumName(Name = "工作组奖励占比")]
        WorkGroupRoyalty,
        /// <summary>
        /// 工作组管理奖励
        /// </summary>
        [EnumName(Name = "工作组管理奖励")]
        WorkGroup
    }
}
