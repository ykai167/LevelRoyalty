using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR.Repositories;

namespace LR.Entity
{
    /// <summary>
    /// 工作组
    /// </summary>
    public partial class WorkGroup : UpdateNamingEntity<Guid, Guid>
    {
        /// <summary>
        /// 妈咪ID
        /// </summary>
        public Guid ManagerID { get; set; }

        /// <summary>
        /// 助理ID
        /// </summary>
        public Guid AssistantID { get; set; }
    }

    public partial class WorkGroup
    {
        public enum WorkGroupState
        {
            [EnumName(Name = "正常")]
            Normal =DataState.Normal,
            [EnumName(Name = "正常")]
            Dismiss = DataState.Delete,
        }
    }
}
