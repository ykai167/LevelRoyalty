using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public enum StaffState
    {
        [EnumName(Name = "在职")]
        Normal = LR.Entity.DataState.Normal,
        [EnumName(Name = "离职")]
        Quit = LR.Entity.DataState.Disable,
        [EnumName(Name = "删除")]
        Delete = LR.Entity.DataState.Delete,
    }

    public enum DataState
    {
        [EnumName(Name = "正常")]
        Normal = LR.Entity.DataState.Normal,
        [EnumName(Name = "删除")]
        Delete = LR.Entity.DataState.Delete,
    }

    public enum AdminState
    {
        [EnumName(Name = "启用")]
        Normal = LR.Entity.DataState.Normal,
        [EnumName(Name = "禁用")]
        Disable = LR.Entity.DataState.Disable,
    }
}
