using LR.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoyaltyConfigService : IUpdateService<LR.Entity.RoyaltyConfig>
    {
        LR.Entity.RoyaltyConfig GetConfig(RoyaltyType type, Guid acceptID, Guid expendID);
    }

    class RoyaltyConfigService : UpdateServiceBase<LR.Entity.RoyaltyConfig>, IRoyaltyConfigService
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
