using LR.Entity;
using LR.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IWorkGroupService : IUpdateService<WorkGroup>
    {
        WorkGroupMemberModel[] GetManagers(Guid staffID);
        OperateResult AddMember(Guid groupID, Guid staffID);
        OperateResult RemoveMember(Guid memberID);
        OperateResult SetManager(Guid memberID, Guid managerCategoryID);
        OperateResult CancelManager(Guid memberID);
        object[] GetAll();
        object[] GetMembers(Guid groupID);
    }

    public interface IWorkGroupMemberService : IQueryService<WorkGroupMember>
    {
        DateTime LastTime();
    }
    class WorkGroupMemberService : DeleteServiceBase<WorkGroupMember>, IWorkGroupMemberService
    {
        public WorkGroupMemberService(Repositories.DataContext context) : base(context)
        {

        }

        public DateTime LastTime()
        {
            return this.Context.Context.Queryable<WorkGroupMember>().Max(p => p.ModifyDate);
        }
    }

    class WorkGroupService : UpdateServiceBase<WorkGroup>, IWorkGroupService
    {
        WorkGroupMemberService memberService;

        public WorkGroupService()
        {
            memberService = new WorkGroupMemberService(this.Context);
        }
        public WorkGroupService(Repositories.DataContext context) : base(context)
        {
            memberService = new WorkGroupMemberService(this.Context);
        }


        public Models.WorkGroupMemberModel[] GetManagers(Guid staffID)
        {
            var member = this.Context.WorkGroupMembers.GetSingle(p => p.StaffID == staffID);
            if (member != null)
            {
                var array = this.Context.Context
                        .Queryable<WorkGroupMember, WorkGroupManagerCategory>((wgm, wmc) => wmc.ID == wgm.CategoryID)
                        .Where((wgm, wmc) => wgm.WorkGroupID == member.WorkGroupID)
                        .Select<WorkGroupMember>()
                        .ToArray();
                return array.Select(wgm => new WorkGroupMemberModel
                {
                    ID = wgm.StaffID,
                    GroupID = wgm.WorkGroupID,
                    Category = WorkGroupManagerCategoryModel.WorkGroupManagerCategories.FirstOrDefault(p => p.ID == wgm.CategoryID)
                }).ToArray();
            }
            else
            {
                return new WorkGroupMemberModel[0];
            }
        }

        public OperateResult AddMember(Guid groupID, Guid staffID)
        {
            var check = CheckGroup(groupID);
            if (!check.Success)
            {
                return check;
            }

            WorkGroupMember member = GetStaff(groupID, staffID);
            if (member != null)
            {
                return new OperateResult("员工已在该组", false);
            }

            try
            {
                this.Context.Context.Ado.BeginTran();
                memberService.Delete(item => item.StaffID == staffID);
                var id = memberService.Insert(new WorkGroupMember
                {
                    WorkGroupID = groupID,
                    StaffID = staffID
                });
                this.Context.Context.Ado.CommitTran();
                return new InsertResult(id);
            }
            catch (Exception e)
            {
                this.Context.Context.Ado.RollbackTran();
                throw e;
            }


        }

        public OperateResult RemoveMember(Guid memberID)
        {
            if (memberID == Guid.NewGuid())
            {
                return new OperateResult("未选择员工", false);
            }
            memberService.Delete(memberID);
            return new OperateResult("操作成功");
        }

        public OperateResult SetManager(Guid memberID, Guid managerCategoryID)
        {
            if (managerCategoryID == new Guid())
            {
                return new OperateResult("管理员选择错误", false);
            }

            //检查是否已设置
            var member = memberService.Single(p => p.ID == memberID);

            var manager = this.GetManager(member.WorkGroupID, managerCategoryID);
            if (manager != null)
            {
                return new OperateResult("该组已设置改类管理员", false);
            }

            memberService.Update(memberID, new { CategoryID = managerCategoryID });
            return new OperateResult();
        }

        public OperateResult CancelManager(Guid memberID)
        {
            memberService.Update(memberID, new { CategoryID = new Guid() });
            return new OperateResult();
        }

        OperateResult CheckGroup(Guid groupID)
        {
            var group = this.Single(p => p.ID == groupID);
            if (group == null)
            {
                return new OperateResult("组不存在", false);
            }
            else
            {
                return new OperateResult();
            }
        }

        WorkGroupMember GetStaff(Guid groupID, Guid staffID)
        {
            return memberService.Single(p => p.WorkGroupID == groupID && p.StaffID == staffID);
        }

        WorkGroupMember GetManager(Guid groupID, Guid categoryID)
        {
            return memberService.Single(p => p.WorkGroupID == groupID && p.CategoryID == categoryID);
        }

        public object[] GetAll()
        {
            return this.Context.Context.Queryable<Entity.WorkGroup, Entity.Admin>((w, a) => w.State == LR.Entity.DataState.Normal && w.OperatorID == a.ID).Select((w, a) => new
            {
                w.ID,
                w.Name,
                w.CreateDate,
                w.ModifyDate,
                Admin = a.Name
            }).ToArray();
        }

        public object[] GetMembers(Guid groupID)
        {
            return this.Context.Context
                .Queryable<Entity.WorkGroupMember, Entity.Staff, Entity.Admin, Entity.WorkGroupManagerCategory>((w, s, a, wc) => new JoinQueryInfos(
                 JoinType.Left, w.StaffID == s.ID,
                 JoinType.Left, w.OperatorID == a.ID,
                 JoinType.Left, w.CategoryID == wc.ID))
                .Where(w => w.State == Entity.DataState.Normal && w.WorkGroupID == groupID)
                .Select((w, s, a, wc) => new
                {
                    MemberID = w.ID,
                    s.Name,
                    ManagerName = wc.Name,
                    Admin = a.Name,
                    w.CreateDate,
                    w.ModifyDate
                }).ToArray();
        }

        public override OperateResult Delete(Guid groupID)
        {
            try
            {
                this.Context.Context.Ado.BeginTran();
                base.Update(groupID, new { State = Entity.DataState.Delete });
                this.memberService.Delete(p => p.WorkGroupID == groupID);
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
