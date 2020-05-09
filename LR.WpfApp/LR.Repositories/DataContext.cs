using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Repositories
{
    public class DataContext : IDisposable
    {
        ISqlSugarClient context;
        public DataContext(bool createTabel = false)
        {
            this.context = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = $"DataSource={Tools.ConfigHelper.AppSettings["sqliteConnectionString"] ?? System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.db")}",
                InitKeyType = InitKeyType.Attribute,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,

            });

            this.context.Aop.OnLogExecuting = (str, arry) =>
            {
            };

            if (createTabel)
            {
                this.context.CodeFirst.InitTables(this.GetType().GetProperties().Where(p => p.PropertyType.IsGenericType).Select(p => p.PropertyType.GetGenericArguments()[0]).ToArray());
            }
        }

        public ISqlSugarClient Context => context;
        public SimpleClient<Entity.Admin> Admins => new SimpleClient<Entity.Admin>(context);
        public SimpleClient<Entity.ConsumeData> ConsumeDatas => new SimpleClient<Entity.ConsumeData>(context);
        public SimpleClient<Entity.Log> Logs => new SimpleClient<Entity.Log>(context);
        public SimpleClient<Entity.Room> Rooms => new SimpleClient<Entity.Room>(context);
        public SimpleClient<Entity.RoomCategory> RoomCategories => new SimpleClient<Entity.RoomCategory>(context);
        public SimpleClient<Entity.Royalty> Royalties => new SimpleClient<Entity.Royalty>(context);
        public SimpleClient<Entity.RoyaltySettle> RoyaltySettles => new SimpleClient<Entity.RoyaltySettle>(context);
        public SimpleClient<Entity.RoyaltyConfig> RoyaltyConfigs => new SimpleClient<Entity.RoyaltyConfig>(context);
        public SimpleClient<Entity.SettleBatch> SettleBatchs => new SimpleClient<Entity.SettleBatch>(context);
        public SimpleClient<Entity.Staff> Staffs => new SimpleClient<Entity.Staff>(context);
        public SimpleClient<Entity.StaffLevel> StaffLevels => new SimpleClient<Entity.StaffLevel>(context);
        public SimpleClient<Entity.WorkGroup> WorkGroups => new SimpleClient<Entity.WorkGroup>(context);
        public SimpleClient<Entity.WorkGroupManagerCategory> WorkGroupManagerCategories => new SimpleClient<Entity.WorkGroupManagerCategory>(context);
        public SimpleClient<Entity.WorkGroupMember> WorkGroupMembers => new SimpleClient<Entity.WorkGroupMember>(context);

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }


}
