using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    public class OperateResult
    {
        public OperateResult(string message = null, bool success = true)
        {
            this.Message = message;
            this.Success = success;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; }
    }

    public class InsertResult : OperateResult
    {
        public InsertResult(Guid newID, string message = null, bool success = true)
        {
            this.NewID = NewID;
        }
        public Guid NewID { get; }
    }
}
