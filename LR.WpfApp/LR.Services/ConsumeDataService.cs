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


    public interface IRoyaltyService : IQueryService<Royalty>
    {
    }

    public interface IConsumeDataService : IUpdateService<ConsumeData>
    {

    }
    public class ConsumeDataService : UpdateServiceBase<ConsumeData>, IConsumeDataService
    {
        ISettleBatchService _settleBatchService;
        public ConsumeDataService(ISettleBatchService settleBatchService)
        {
            this._settleBatchService = settleBatchService;
        }
        public override Guid Insert(ConsumeData entity)
        {
            if (entity.ID == new Guid())
            {
                entity.ID = Guid.NewGuid();
            }
            var num = this._settleBatchService.GetCurrentNum();
            //消费记录本人
            var staff = MemoryData.Current.Staffs.First(p => p.ID == entity.StaffID);

            Action<SqlSugar.ISqlSugarClient, Models.RoyaltyConfigModel, Models.StaffModel> insert = (db, config, accept) =>
            {
                db.Insertable<Royalty>(new Royalty
                {
                    ID = Guid.NewGuid(),
                    ConsumeDataID = entity.ID,
                    StaffID = accept.ID,
                    RoyaltyType = (int)config.RoyaltyType,
                    Percent = config.Percent,
                    SettleNum = num,
                }).ExecuteCommand();
            };

            using (var context = this.Context.Context)
            {
                try
                {
                    context.Ado.BeginTran();
                    var consumeDataID = context.Insertable<ConsumeData>(entity).ExecuteCommand();
                    //增加消费记录
                    //1.订房奖励
                    var reservationConfig = MemoryData.Current.RoyaltyConfigs[RoyaltyType.Reservation, staff.Level];
                    if (reservationConfig != null)
                    {
                        insert(context, reservationConfig, staff);
                    }

                    //2.管理奖励,
                    if (staff.Referrer != null)
                    {
                        var administrationConfig = MemoryData.Current.RoyaltyConfigs[RoyaltyType.Administration, staff.Referrer.Level, staff.Level];
                        if (administrationConfig != null)
                        {
                            insert(context, administrationConfig, staff.Referrer);
                        }
                    }

                    //3.协作奖励,超越奖励
                    //3.1找到上级,
                    //上级是最低级别,不处理;不是,查看上级的上级是否与
                    var current = staff.Referrer;
                    while (current != null && current.Referrer != null)
                    {
                        var refferrer = current.Referrer;
                        if (current.Level != Models.LevelModel.Min && refferrer.Level >= current.Level)
                        {
                            var type = refferrer.Level == current.Level ? RoyaltyType.Cooperation : RoyaltyType.Transcend;
                            var config = MemoryData.Current.RoyaltyConfigs[type, refferrer.Level, current.Level];
                            if (config != null)
                            {
                                insert(context, config, refferrer);
                            }
                        }
                        current = refferrer;
                    }

                    //4.工作组管理人员奖励
                    //4.1找到自己的工作组
                    //4.2找到工作组的管理员
                    //4.3为每一个管理员根据管理人员类别记录奖励数据



                    this.Context.Context.Ado.CommitTran();
                    return entity.ID;
                }
                catch (Exception e)
                {
                    context.Ado.RollbackTran();
                    throw e;
                }
            }


        }
    }
}
