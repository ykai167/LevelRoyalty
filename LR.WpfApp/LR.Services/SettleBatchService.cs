using LR.Entity;
using LR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface ISettleBatchService : IQueryService<SettleBatch>
    {
    }
    public class SettleBatchService : UpdateServiceBase<SettleBatch>, ISettleBatchService
    {
        public SettleBatchService(Repositories.DataContext context) : base(context)
        {

        }
        /// <summary>
        /// 获取或生成一个账期号
        /// </summary>
        /// <returns></returns>
        internal SettleBatch GetOrGenCurrent()
        {
            var current = this.Single(p => !p.IsHistory);
            if (current == null)
            {
                var now = DateTime.Now;

                var last = this.Queryable.OrderBy(p => p.CreateDate, SqlSugar.OrderByType.Desc).First();
                Func<int> fnNum = () =>
                {
                    return now.Year * 10000 + now.Month * 100 + 1;
                };
                int num = 0;
                if (last == null)
                {
                    num = fnNum();
                }
                else
                {
                    num = last.Num / 10000 == now.Year && (last.Num - now.Year * 10000) / 100 == now.Month ? last.Num + 1 : fnNum();
                }

                this.Insert(current = new SettleBatch
                {
                    Num = num,
                    StartTime = now,
                    IsHistory = false,
                    State = (int)DataState.Normal
                });
            }
            return current;
        }
    }
}
