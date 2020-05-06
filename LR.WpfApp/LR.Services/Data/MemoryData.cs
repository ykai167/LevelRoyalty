using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LR.Models;

namespace LR.Services
{
    class RoyaltyConfigs
    {
        IEnumerable<RoyaltyConfigModel> data;
        public RoyaltyConfigs()
        {
            var royaltyConfigList = Tools.DIHelper.GetInstance<IRoyaltyConfigService>().List();
            var temp = new List<RoyaltyConfigModel>();
            var array = new[] {
                (int)RoyaltyType.Reservation,
                (int)RoyaltyType.Administration,
                (int)RoyaltyType.Cooperation,
                (int)RoyaltyType.Transcend };
            foreach (var item in royaltyConfigList.Where(p => array.Contains(p.RoyaltyType)))
            {
                temp.Add(new RoyaltyConfigModel
                {
                    RoyaltyType = (RoyaltyType)item.RoyaltyType,
                    AcceptID = item.AcceptID,
                    ExpendID = item.ExpendID,
                    Percent = item.Percent / 100
                });
            }

            var workGrouproyalty = royaltyConfigList.FirstOrDefault(p => p.RoyaltyType == (int)RoyaltyType.WorkGroupRoyalty);
            if (workGrouproyalty != null)
            {
                foreach (var item in royaltyConfigList.Where(p => p.RoyaltyType == (int)RoyaltyType.WorkGroup))
                {
                    temp.Add(new RoyaltyConfigModel
                    {
                        RoyaltyType = (RoyaltyType)item.RoyaltyType,
                        AcceptID = item.AcceptID,
                        ExpendID = item.ExpendID,
                        Percent = (item.Percent / 100) * (workGrouproyalty.Percent / 100)
                    });
                }
            }
            this.data = temp;
        }

        public RoyaltyConfigModel this[RoyaltyType type, LevelModel accept]
        {
            get { return data.FirstOrDefault(p => p.RoyaltyType == type && p.AcceptID == accept.ID); }
        }
        public RoyaltyConfigModel this[RoyaltyType type, LevelModel accpt, LevelModel expend]
        {
            get { return data.FirstOrDefault(p => p.RoyaltyType == type && p.AcceptID == accpt.ID && p.ExpendID == expend.ID); }
        }
        public RoyaltyConfigModel this[RoyaltyType type, Models.WorkGroupManagerCategoryModel catetory]
        {
            get { return data.FirstOrDefault(p => p.RoyaltyType == type && p.AcceptID == catetory.ID); }
        }
    }

    public class MemoryData
    {
        public IEnumerable<StaffModel> Staffs;

        internal readonly RoyaltyConfigs RoyaltyConfigs;

        public

        MemoryData()
        {
            this.ReloadStaffs();
            this.RoyaltyConfigs = new RoyaltyConfigs();
        }

        public void ReloadStaffs()
        {
            this.Staffs = Tools.DIHelper.GetInstance<IStaffService>().List().Select(item => new StaffModel
            {
                ID = item.ID,
                Name = item.Name,
                ReferrerID = item.ReferrerID
            }).ToArray();

            foreach (var item in this.Staffs)
            {
                item.Subs = this.Staffs.Where(p => p.ReferrerID == item.ID).ToArray();
                item.Referrer = this.Staffs.FirstOrDefault(p => p.ID == item.ReferrerID);
            }
        }

        static MemoryData current = new MemoryData();
        public static MemoryData Current { get { return current; } }
    }
}
