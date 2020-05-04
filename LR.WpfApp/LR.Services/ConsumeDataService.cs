using LR.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{



    public interface IConsumeDataService : IUpdateService<ConsumeData>
    {

    }
    public class ConsumeDataService : UpdateServiceBase<ConsumeData>, IConsumeDataService
    {
        ISettleBatchService _settleBatchService;
        IWorkGroupService _workGroupService;
        public ConsumeDataService(ISettleBatchService settleBatchService, IWorkGroupService workGroupService)
        {
            this._settleBatchService = settleBatchService;
            this._workGroupService = workGroupService;
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
            //工作组管理员
            var managers = this._workGroupService.GetManagers(staff.ID);

            var royaltyService = new RoyaltyService(this.Context);

            Action<Models.RoyaltyConfigModel, Models.StaffModel> insert = (config, accept) =>
           {
               royaltyService.Insert(new Royalty
               {
                   ConsumeDataID = entity.ID,
                   StaffID = accept.ID,
                   RoyaltyType = (int)config.RoyaltyType,
                   Percent = config.Percent,
                   SettleNum = num,
               });
           };


            try
            {
                this.Context.Context.Ado.BeginTran();
                var consumeDataID = base.Insert(entity);
                //增加消费记录
                //1.订房奖励
                var reservationConfig = MemoryData.Current.RoyaltyConfigs[RoyaltyType.Reservation, staff.Level];
                if (reservationConfig != null)
                {
                    insert(reservationConfig, staff);
                }

                //2.管理奖励,
                if (staff.Referrer != null)
                {
                    var administrationConfig = MemoryData.Current.RoyaltyConfigs[RoyaltyType.Administration, staff.Referrer.Level, staff.Level];
                    if (administrationConfig != null)
                    {
                        insert(administrationConfig, staff.Referrer);
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
                            insert(config, refferrer);
                        }
                    }
                    current = refferrer;
                }

                //4.工作组管理人员奖励
                //为每一个管理员根据管理人员类别记录奖励数据
                foreach (var item in managers)
                {
                    var config = MemoryData.Current.RoyaltyConfigs[RoyaltyType.WorkGroup, item.Category];
                    if (config != null)
                    {
                        insert(config, item);
                    }
                }


                this.Context.Context.Ado.CommitTran();
                return entity.ID;
            }
            catch (Exception e)
            {
                this.Context.Context.Ado.RollbackTran();
                throw e;
            }
        }
    }
}
