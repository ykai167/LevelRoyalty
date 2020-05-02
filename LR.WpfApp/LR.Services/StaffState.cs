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
        Normal,
        [EnumName(Name = "离职")]
        Quit,
    }

    public enum DataState
    {
        [EnumName(Name = "正常")]
        Normal,
        [EnumName(Name = "删除")]
        Delete,
    }
}
