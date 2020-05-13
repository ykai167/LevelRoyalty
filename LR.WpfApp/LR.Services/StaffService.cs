using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IStaffService : IUpdateService<LR.Entity.Staff>
    {
        object[] GetStaffs();
    }

    class StaffService : UpdateServiceBase<LR.Entity.Staff>, IStaffService
    {
        public object[] GetStaffs()
        {
            var data = this.Context.Context.Queryable<
                LR.Entity.Staff,
                LR.Entity.WorkGroupMember,
                LR.Entity.WorkGroup,
                LR.Entity.Admin>((s, wgm, wg, a) => new JoinQueryInfos(
                JoinType.Left, s.ID == wgm.StaffID,
                JoinType.Left, wgm.WorkGroupID == wg.ID,
                JoinType.Inner, s.OperatorID == a.ID))
                .Where(s => s.State != Entity.DataState.Delete)
                .Select((s, wgm, wg, a) => new
                {
                    s.ID,
                    s.No,
                    s.Name,
                    s.IdenNo,
                    s.MobileNo,
                    s.EntryTime,
                    s.CreateDate,
                    s.ModifyDate,
                    s.State,
                    s.ReferrerID,
                    wgm.WorkGroupID,
                    GroupName = wg.Name,
                    Admin = a.Name
                }).ToArray();
            return data.Select(p => new
            {
                p.ID,
                p.No,
                p.Name,
                p.IdenNo,
                p.MobileNo,
                p.EntryTime,
                p.CreateDate,
                p.ModifyDate,
                p.GroupName,
                p.Admin,
                p.ReferrerID,
                p.WorkGroupID,
                p.State,
                StateName = ((Services.StaffState)p.State).GetName(),
                Level = MemoryData.Current.Staffs.FirstOrDefault(item => item.ID == p.ID)?.Level?.Name,
                Referrer = MemoryData.Current.Staffs.FirstOrDefault(item => item.ID == p.ID)?.Referrer?.Name
            }).ToArray();
        }

        public override void Update(Guid id, object columData)
        {
            base.Update(id, columData);
            MemoryData.Current.ReloadStaffs();
        }
        public override Guid Insert(Entity.Staff entity)
        {
            var guid = base.Insert(entity);
            MemoryData.Current.ReloadStaffs();
            return guid;
        }
    }
}
