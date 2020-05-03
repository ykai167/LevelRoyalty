using LR.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IConsumeDataService : IService<ConsumeData>
    {

    }
    public class ConsumeDataService : ServiceBase<ConsumeData>, IConsumeDataService
    {
        public override void Insert(ConsumeData entity)
        {
            base.Insert(entity);


        }
    }

    public interface IStaffService : IService<LR.Entity.Staff>
    {
    }

    public class StaffService : ServiceBase<LR.Entity.ConsumeData>, IConsumeDataService
    {
    }

    public interface IStaffLevelService : IService<LR.Entity.StaffLevel>
    {
    }

    public class StaffLevelService : ServiceBase<LR.Entity.StaffLevel>, IStaffLevelService
    {
        public StaffLevelService()
        {

        }
    }

    public interface IWorkGroupManagerCategoryService : IService<LR.Entity.WorkGroupManagerCategory>
    {
    }

    public class WorkGroupManagerCategoryService : ServiceBase<LR.Entity.WorkGroupManagerCategory>, IWorkGroupManagerCategoryService
    {

    }

    public interface IRoyaltyConfigService : IService<LR.Entity.RoyaltyConfig>
    {
        LR.Entity.RoyaltyConfig GetConfig(RoyaltyType type, Guid acceptID, Guid expendID);
    }

    public class RoyaltyConfigService : ServiceBase<LR.Entity.RoyaltyConfig>, IRoyaltyConfigService
    {
        public RoyaltyConfig GetConfig(RoyaltyType type, Guid acceptID, Guid expendID)
        {
            var entity = this.Context.RoyaltyConfigs.GetSingle(p => p.RoyaltyType == (int)type && p.AcceptID == acceptID && p.ExpendID == expendID);
            if (entity == null)
            {
                this.Context.RoyaltyConfigs.Insert(entity = new RoyaltyConfig
                {
                    ID = Guid.NewGuid(),
                    AcceptID = acceptID,
                    ExpendID = expendID,
                    Percent = 0,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    RoyaltyType = (int)type,
                    State = (int)DataState.Normal
                });
            }
            return entity;
        }
    }
}
