using LR.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface ISettleBatchService : IUpdateService<SettleBatch>
    {
        int GetCurrentNum();
    }

    public class SettleBatchService : UpdateServiceBase<SettleBatch>, ISettleBatchService
    {
        public int GetCurrentNum()
        {
            var temp = this.Context.SettleBatchs.GetSingle(p => !p.IsHistory);
            if (temp == null)
            {
                var now = DateTime.Now;
                this.Insert(temp = new SettleBatch
                {
                    Num = now.Year * 10000 + now.Month * 100 + 1,
                    StartTime = now,
                    IsHistory = false,
                    State = (int)DataState.Normal
                });
            }
            return temp.Num;
        }
    }
}
