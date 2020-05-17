using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface ILogService : IQueryService<LR.Entity.Log>
    {
        LR.Tools.Pager<object> GetPage(int pageIndex, int pageSize);
    }

    class LogService : QueryServiceBase<LR.Entity.Log>, ILogService
    {

        public LR.Tools.Pager<object> GetPage(int pageIndex, int pageSize)
        {
            var query = this.Context.Context.Queryable<Entity.Log, Entity.Admin>((log, admin) => new SqlSugar.JoinQueryInfos(SqlSugar.JoinType.Left, log.OperatorID == admin.ID))
                .Select((log, admin) => new LogModel
                {
                    Type = (LogType)log.Type,
                    Operator = admin.Name,
                    Table = log.Table,
                    Data = log.Data,
                    CreateDate = log.CreateDate
                });
            return new Tools.Pager<object>(query.OrderBy(log => log.CreateDate, SqlSugar.OrderByType.Desc).ToPageList(pageIndex, pageSize), pageSize, query.Count());
        }
    }

    public class LogModel
    {
        public String Operator { get; set; }
        public LogType Type { get; set; }
        public String Table { get; set; }
        public String Data { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
