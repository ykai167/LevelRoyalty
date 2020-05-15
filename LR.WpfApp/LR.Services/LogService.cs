using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface ILogService : IQueryService<LR.Entity.Log>
    {
        List<object> GetAll();
    }

    class LogService : QueryServiceBase<LR.Entity.Log>, ILogService
    {
        public List<object> GetAll()
        {
            return this.Context.Context.Queryable<Entity.Log, Entity.Admin>((log, admin) => log.OperatorID == admin.ID)
                 .Select((log, admin) => (object)new
                 {
                     Operator = admin.Name,
                     Table = log.Table,
                     Data = log.Data,
                     CreateDate = log.CreateDate,
                 }).ToList();
        }
    }
}
