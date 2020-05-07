﻿using LR.Entity;
using LR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }

    public class RoyaltySettleService : UpdateServiceBase<RoyaltySettle>, IRoyaltySettleService
    {
        public override Guid Insert(RoyaltySettle entity)
        {
            throw new Exception("非法操作");
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
                    Total = item.Total
                }).ToArray());

                var updateObj = new { };
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
