using LR.Entity;
using LR.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{



    public interface IConsumeDataService : IUpdateService<ConsumeData>
    {
        LR.Tools.Pager<object> GetPage(int pageIndex, int pageSize);
        List<ConsumeDataModel> GetExtractList(DateTime start, DateTime end);
        void Delete(Guid id);
    }
    class ConsumeDataService : UpdateServiceBase<ConsumeData>, IConsumeDataService
    {
        IWorkGroupService _workGroupService;
        public ConsumeDataService()
        {
            this._workGroupService = new WorkGroupService(this.Context);
        }


        public void Delete(Guid id)
        {
            try
            {
                this.Context.Context.Ado.BeginTran();
                this.Context.Context.Deleteable<Entity.Royalty>(p => p.ConsumeDataID == id).ExecuteCommand();
                this.Update(id, new { State = Entity.DataState.Delete });
                this.Context.Context.Ado.CommitTran();
            }
            catch (Exception e)
            {
                this.Context.Context.Ado.RollbackTran();
                throw e;
            }
        }

        public override void Update(Guid id, object columData)
        {
            //账期不一致不可修改
            var old = this.Single(id);
            if (old == null)
            {
                throw new Exception("不存在记录");
            }

            if (old.CreateDate < MemoryData.Current.LastTime)
            {
                throw new Exception("配置数据,员工等级,工作组已发生变化,不可修改");
            }

            var currentSettle = new SettleBatchService(this.Context).GetOrGenCurrent();
            if (old.SettleNum != currentSettle.Num)
            {
                throw new Exception("已结算记录不可修改");
            }

            var type = columData.GetType();
            Guid roomID = (Guid)(type.GetProperty(nameof(old.RoomID))?.GetValue(columData) ?? old.RoomID);
            decimal minC;
            var result = this.CheckRomm(roomID, out minC);
            if (!result.Success)
            {
                throw new Exception(result.Message);
            }

            decimal amount = (decimal)(type.GetProperty(nameof(old.Amount))?.GetValue(columData) ?? old.Amount);
            Guid staffID = (Guid)(type.GetProperty(nameof(old.StaffID))?.GetValue(columData) ?? old.StaffID);
            //消费记录本人
            var staff = MemoryData.Current.Staffs.First(p => p.ID == staffID);
            //工作组管理员
            var managers = this._workGroupService.GetManagers(staff.ID);

            //如果没有达到最低消费,与该id关联的所有消费全部删除,否则只修改本记录即可
            try
            {
                this.Context.Context.Ado.BeginTran();
                base.Update(id, columData);
                this.Context.Context.Deleteable<Royalty>(p => p.ConsumeDataID == id).ExecuteCommand();
                if (amount >= minC)
                {
                    //增加消费记录
                    AddRoy(old.ID, staff, managers, currentSettle.Num);
                }
                this.Context.Context.Ado.CommitTran();
            }
            catch (Exception e)
            {
                this.Context.Context.Ado.RollbackTran();
                throw e;
            }
        }

        public override Guid Insert(ConsumeData entity)
        {
            if (entity.ID == new Guid())
            {
                entity.ID = Guid.NewGuid();
            }

            decimal minC;
            var result = this.CheckRomm(entity.RoomID, out minC);
            if (!result.Success)
            {
                throw new Exception(result.Message);
            }

            var currentSettle = new SettleBatchService(this.Context).GetOrGenCurrent();
            entity.SettleNum = currentSettle.Num;
            //消费记录本人
            var staff = MemoryData.Current.Staffs.First(p => p.ID == entity.StaffID);
            //工作组管理员
            var managers = this._workGroupService.GetManagers(staff.ID);

            try
            {
                this.Context.Context.Ado.BeginTran();
                var consumeDataID = base.Insert(entity);
                if (entity.Amount >= minC)
                {
                    //增加消费记录
                    AddRoy(entity.ID, staff, managers, currentSettle.Num);
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

        Models.OperateResult CheckRomm(Guid roomID, out decimal MinCharge)
        {
            //是否达到最低消费
            var room = new RoomService().Single(roomID);
            if (room == null)
            {
                MinCharge = 0;
                return new Models.OperateResult("未找到房间", false);
            }

            var category = new RoomCategoryService().Single(room.CategoryID);
            if (category == null)
            {
                MinCharge = 0;
                return new Models.OperateResult("房间未设置分类");
            }
            MinCharge = category.MinCharge;
            return new Models.OperateResult();
        }

        void AddRoy(Guid consumeDataID, Models.StaffModel staff, Models.WorkGroupMemberModel[] managers, int num)
        {
            Action<Models.RoyaltyConfigModel, Models.StaffBase> insert = (config, accept) =>
            {
                this.Context.Royalties.Insert(new Royalty
                {
                    ConsumeDataID = consumeDataID,
                    StaffID = accept.ID,
                    RoyaltyType = (int)config.RoyaltyType,
                    Percent = config.Percent,
                    SettleNum = num,
                });
            };

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
                if (current.Level != Models.LevelModel.Min && refferrer.Level != Models.LevelModel.Min && refferrer.Level <= current.Level)
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
        }

        public List<ConsumeDataModel> GetExtractList(DateTime start, DateTime end)
        {
            var query = this.Context.Context.Queryable<Entity.ConsumeData, Entity.Room, Entity.Staff, Entity.Admin>((d, r, s, a) => d.RoomID == r.ID && d.StaffID == s.ID && d.OperatorID == a.ID)
                .Where(d => d.CreateDate >= start && d.CreateDate <= end)
                .Select((d, r, s, a) => new ConsumeDataModel
                {
                    ID = d.ID,
                    Amount = d.Amount,
                    CreateDate = d.CreateDate,
                    ModifyDate = d.ModifyDate,
                    RoomID = r.ID,
                    RoomNo = r.No,
                    RoomName = r.Name,
                    StaffID = s.ID,
                    StaffName = s.Name,
                    StaffNo = s.No,
                    Admin = a.Name
                });
            return query.ToList();
        }

        public LR.Tools.Pager<object> GetPage(int pageIndex, int pageSize)
        {
            var query = this.Context.Context.Queryable<Entity.ConsumeData, Entity.Room, Entity.Staff, Entity.Admin>((d, r, s, a) => d.RoomID == r.ID && d.StaffID == s.ID && d.OperatorID == a.ID)
                .Select((d, r, s, a) => new ConsumeDataModel
                {
                    ID = d.ID,
                    Amount = d.Amount,
                    CreateDate = d.CreateDate,
                    ModifyDate = d.ModifyDate,
                    RoomID = r.ID,
                    RoomNo = r.No,
                    RoomName = r.Name,
                    StaffID = s.ID,
                    StaffName = s.Name,
                    StaffNo = s.No,
                    Admin = a.Name
                });
            return new Tools.Pager<object>(query.OrderBy(d => d.CreateDate, SqlSugar.OrderByType.Desc).ToPageList(pageIndex, pageSize), pageSize, query.Count());
        }
    }

    public class ConsumeDataModel
    {
        public Guid ID { get; set; }
        decimal a;
        public decimal Amount { get { return a.Places(2); } set { a = value; } }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public Guid RoomID { get; set; }
        public string RoomNo { get; set; }
        public string RoomName { get; set; }
        public Guid StaffID { get; set; }
        public string StaffName { get; set; }
        public string StaffNo { get; set; }
        public string Admin { get; set; }

    }
}
