﻿using LR.Entity;
using LR.Models;
using LR.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    /// <summary>
    /// 提成结算
    /// </summary>
    public interface IRoyaltySettleService : IUpdateService<RoyaltySettle>
    {
        /// <summary>
        /// 当前账期结算
        /// </summary>
        OperateResult Settlement();

        RoyaltyStatisticsModel[] GetSettles(int settleNum);
    }

    class RoyaltySettleService : UpdateServiceBase<RoyaltySettle>, IRoyaltySettleService
    {
        public override Guid Insert(RoyaltySettle entity)
        {
            throw new Exception("非法操作");
        }

        public RoyaltyStatisticsModel[] GetSettles(int settleNum)
        {
            return this.Context.Context.Queryable<RoyaltySettle, Staff, Admin>((r, s, a) => new SqlSugar.JoinQueryInfos(
                SqlSugar.JoinType.Inner, r.StaffID == s.ID,
                SqlSugar.JoinType.Left, r.OperatorID == a.ID))
                .Where(r=>r.SettleNum==settleNum)
                .Select((r, s, a) => new
                {
                    s.No,
                    ID = r.ID,
                    StaffID = r.StaffID,
                    StaffName = s.Name,
                    r.Json,
                    CreateDate = r.CreateDate,
                    IsExpend = r.IsExpend,
                    IsSelf = r.IsSelf,
                    Receiver = r.Receiver,
                    ModifyDate = r.ModifyDate,
                    Admin = a.Name
                }).ToArray()
                .Select((r) => new RoyaltyStatisticsModel
                {
                    ID = r.ID,
                    StaffNo = r.No,
                    StaffID = r.StaffID,
                    StaffName = r.StaffName,
                    Items = r.Json.JsonTo<KeyValuePair<RoyaltyType, decimal>[]>(),
                    CreateDate = r.CreateDate,
                    IsExpend = r.IsExpend,
                    IsSelf = r.IsSelf,
                    Receiver = r.Receiver,
                    ModifyDate = r.ModifyDate,
                    Admin = r.Admin
                }).ToArray();
        }

        public OperateResult Settlement()
        {
            var currentSettle = new SettleBatchService(this.Context).GetOrGenCurrent();

            var data = new RoyaltyService().Statistics(currentSettle.Num);

            var settleBatchService = new SettleBatchService(this.Context);
            try
            {
                this.Context.Context.Ado.BeginTran();

                //1.将计算数据写入
                this.Context.RoyaltySettles.InsertRange(data.Select(item => new RoyaltySettle
                {
                    ID = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    IsExpend = false,
                    IsSelf = false,
                    SettleNum = currentSettle.Num,
                    StaffID = item.StaffID,
                    State = (int)DataState.Normal,
                    Json = item.Items.LogJson()
                }).ToArray());

                //2.当前账期结束
                settleBatchService.Update(currentSettle.ID, new
                {
                    IsHistory = true,
                    EndTime = DateTime.Now
                });
                //3.开始新的账期
                settleBatchService.GetOrGenCurrent();

                this.Context.Context.Ado.CommitTran();
                return new OperateResult();
            }
            catch (Exception e)
            {
                this.Context.Context.Ado.RollbackTran();
                throw e;
            }
        }
    }
}
