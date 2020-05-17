using LR.Entity;
using LR.Tools;
using SqlSugar;
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

            var logHelper = LogHelper.Current;

            return new Tools.Pager<object>(query.OrderBy(log => log.CreateDate, SqlSugar.OrderByType.Desc).ToPageList(pageIndex, pageSize).Select(p => new
            {
                CreateDate = p.CreateDate,
                Operator = p.Operator,
                Table = logHelper.Tables.FirstOrDefault(t => t.Table == p.Table)?.Name ?? p.Table,
                Type = p.Type.GetName(),
                Data = string.Join("\r\n", p.Data.JsonTo()
                .Cast<Newtonsoft.Json.Linq.JProperty>()
                .Select(prop =>
                {
                    var f = logHelper.Tables.FirstOrDefault(t => t.Table == p.Table)?.Fields?.FirstOrDefault(fi => fi.Name == prop.Name) ?? logHelper.Global.FirstOrDefault(c => c.Name == prop.Name);
                    if (f != null)
                    {
                        object value = null;
                        if (f.Map != null)
                        {
                            value = this.Context.Context.SqlQueryable<Temp>($"select * from `{f.Map.Table}`").Where(it => it.ID == (Guid)prop.Value).Single()?.Name;
                        }
                        else if (f.Enum != null)
                        {
                            value = typeof(Extends).GetMethod("GetName").Invoke(null, new[] {
                                Enum.Parse(Type.GetType(f.Enum.Type),prop.Value.ToString())
                            });
                        }
                        else
                        {
                            value = prop.Value;
                        }
                        return $"{f.Header}:{value}";
                    }
                    else
                    {
                        return $"{prop.Name}:{prop.Value}";
                    }
                }))
            }), pageSize, query.Count());
        }
    }

    public class Temp
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class LogModel
    {
        public String Operator { get; set; }
        public LogType Type { get; set; }
        public String Table { get; set; }
        public String Data { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class LogHelper
    {
        public static LogHelper Current
        {
            get
            {
                var json = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "logconfig.json"));
                var result = json.JsonTo<LogHelper>();
                return result;
            }
        }
        public Filed[] Global { get; set; }
        public Loger[] Tables { get; set; }
    }

    public class Loger
    {
        public string Table { get; set; }
        public string Name { get; set; }
        public Filed[] Fields { get; set; }
    }

    public class Filed
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// 外链表
        /// </summary>
        public Map Map { get; set; }
        /// <summary>
        /// 枚举字段
        /// </summary>
        public EnumType Enum { get; set; }
    }

    public class EnumType
    {
        public string Type { get; set; }
    }

    public class Map
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Table { get; set; }
    }
}
