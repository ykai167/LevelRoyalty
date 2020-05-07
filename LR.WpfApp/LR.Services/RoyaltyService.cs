using LR.Entity;
using LR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoyaltyService : IQueryService<Royalty>
    {
        Models.RoyaltyModel[] Detaile(Guid staffID, int settleNum);
        List<RoyaltyStatisticsModel> Statistics(int settleNum);
    }

    public class RoyaltyService : InsertServiceBase<Royalty>, IRoyaltyService
    {
        public RoyaltyService()
        {

        }

        public RoyaltyService(Repositories.DataContext context) : base(context)
        {

        }

        public RoyaltyModel[] Detaile(Guid staffID, int settleNum)
        {
            var data = this.Context.Context
                .Queryable<ConsumeData, Royalty, Staff>((c, r, s) => c.ID == r.ConsumeDataID && c.StaffID == s.ID)
                .Where((c, r, s) => r.StaffID == staffID && r.SettleNum == settleNum)
                .Select((c, r, s) => new RoyaltyModel
                {
                    StaffID = r.StaffID,
                    StaffName = s.Name,
                    Amount = c.Amount,
                    CreateTime = r.CreateDate,
                    Percent = r.Percent,
                    SettleNum = r.SettleNum,
                    RoyaltyType = (RoyaltyType)r.RoyaltyType
                }).ToArray();

            return data;
        }

        public List<RoyaltyStatisticsModel> Statistics(int settleNum)
        {
            var data = this.Context.Context
                .Queryable<ConsumeData, Royalty, Staff>((c, r, s) => c.ID == r.ConsumeDataID && c.StaffID == s.ID)
                .Where((c, r, s) => r.SettleNum == settleNum)
                .Select((c, r, s) => new RoyaltyModel
                {
                    StaffID = r.StaffID,
                    StaffName = s.Name,
                    Amount = c.Amount,
                    CreateTime = r.CreateDate,
                    Percent = r.Percent,
                    SettleNum = r.SettleNum,
                    RoyaltyType = (RoyaltyType)r.RoyaltyType
                }).ToArray();

            return data.GroupBy(item => new { item.StaffID, item.StaffName })
                .Select(g => new RoyaltyStatisticsModel
                {
                    StaffID = g.Key.StaffID,
                    StaffName = g.Key.StaffName,
                    SettleNum = settleNum,
                    Total = g.Sum(p => p.Royalty)
                }).ToList();
        }
    }
}
