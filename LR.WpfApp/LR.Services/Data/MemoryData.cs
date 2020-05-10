using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR.Entity;
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

    public static class StaffsExtends
    {
        /// <summary>
        /// 获取可以作为上级的人
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<StaffModel> GetReferrers(this IEnumerable<StaffModel> all, Guid staffID)
        {
            List<Guid> list = new List<Guid>();
            var currenty = all.First(p => p.ID == staffID);
            Add(currenty, list);
            return all.Where(p => p.ID != staffID && !list.Contains(p.ID));
        }

        static void Add(StaffModel current, List<Guid> list)
        {
            if (current.Subs != null)
            {
                foreach (var item in current.Subs)
                {
                    list.Add(item.ID);
                    Add(item, list);
                }
            }
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
            this.Staffs = Tools.DIHelper.GetInstance<IStaffService>().List(p => p.State == LR.Entity.DataState.Normal)
                .Select(item => new StaffModel
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
