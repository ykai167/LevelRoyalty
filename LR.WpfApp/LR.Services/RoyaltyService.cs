﻿using LR.Entity;
using LR.Models;
using LR.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoyaltyService : IQueryService<Royalty>
    {
        List<object> Detaile(Guid staffID, int settleNum);
        List<RoyaltyStatisticsModel> Statistics(int settleNum);
    }

    class RoyaltyService : InsertServiceBase<Royalty>, IRoyaltyService
    {
        public RoyaltyService()
        {

        }

        public RoyaltyService(Repositories.DataContext context) : base(context)
        {

        }

        public List<object> Detaile(Guid staffID, int settleNum)
        {
            var data = this.Context.Context
                .Queryable<Royalty, ConsumeData, Staff>((r, c, s) => c.ID == r.ConsumeDataID && c.StaffID == s.ID)
                .Where((r, c, s) => r.StaffID == staffID && r.SettleNum == settleNum)
                .OrderBy(r => r.RoyaltyType)
                .Select((r, c, s) => new
                {
                    StaffName = s.Name,
                    c.Amount,
                    r.CreateDate,
                    r.Percent,
                    RoyaltyType = (RoyaltyType)r.RoyaltyType,
                }).ToArray()
                .Select(p => (object)new
                {
                    p.StaffName,
                    Amount = p.Amount.Places(2),
                    p.CreateDate,
                    Percent = p.Percent.Places(4),
                    RoyaltyType = p.RoyaltyType.GetName(),
                    Total = (p.Amount * p.Percent).Places(2)
                }).ToList();

            return data;
        }

        public List<RoyaltyStatisticsModel> Statistics(int settleNum)
        {
            var data = this.Context.Context
                .Queryable<Royalty, ConsumeData, Staff>((r, c, s) => c.ID == r.ConsumeDataID && r.StaffID == s.ID)
                .Where((r, c, s) => r.SettleNum == settleNum)
                .Select((r, c, s) => new _RoyaltyModel
                {
                    StaffID = r.StaffID,
                    StaffNo = s.No,
                    StaffName = s.Name,
                    Amount = c.Amount,
                    CreateTime = r.CreateDate,
                    Percent = r.Percent,
                    SettleNum = r.SettleNum,
                    RoyaltyType = (RoyaltyType)r.RoyaltyType
                }).ToArray();

            return data.GroupBy(item => new { item.StaffID, item.StaffName, item.StaffNo })
                .Select(g => new RoyaltyStatisticsModel
                {
                    StaffID = g.Key.StaffID,
                    StaffNo = g.Key.StaffNo,
                    StaffName = g.Key.StaffName,
                    SettleNum = settleNum,
                    Items = g.GroupBy(p => p.RoyaltyType)
                    .Select(p => new KeyValuePair<RoyaltyType, decimal>(p.Key, p.Sum(r => r.Royalty))).ToArray()
                }).ToList();
        }
    }
}
