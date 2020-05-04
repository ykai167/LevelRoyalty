using LR.Entity;
using LR.Models;
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
        OperateResult RemoveMember(Guid groupID, Guid staffID);
        OperateResult SetManager(Guid groupID, Guid staffID, Guid managerCategoryID);
        OperateResult CancelManager(Guid groupID, Guid staffID, Guid managerCategoryID);
    }

    class WorkGroupMemberService : UpdateServiceBase<WorkGroupMember>
    {
        public WorkGroupMemberService(Repositories.DataContext context) : base(context)
        {

        }
    }

    public class WorkGroupService : UpdateServiceBase<WorkGroup>, IWorkGroupService
    {
        public WorkGroupService()
        {

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
                    GroupID = wgm.WorkGroupID,
                    StaffID = wgm.StaffID,
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

            var service = new WorkGroupMemberService(this.Context);

            WorkGroupMember member = GetStaff(service, groupID, staffID);
            if (member != null)
            {
                return new OperateResult("员工已在该组", false);
            }

            var id = service.Insert(new WorkGroupMember
            {
                WorkGroupID = groupID,
                StaffID = staffID
            });
            return new InsertResult(id);
        }

        public OperateResult RemoveMember(Guid groupID, Guid staffID)
        {
            var chack = CheckGroup(groupID);
            if (!chack.Success)
            {
                return chack;
            }

            var service = new WorkGroupMemberService(this.Context);
            var member = GetStaff(service, groupID, staffID);
            if (member == null)
            {
                return new OperateResult("组内不存在该员工", false);
            }

            service.Update(member.ID, new { State = DataState.Delete });
            return new OperateResult("操作成功");
        }

        public OperateResult SetManager(Guid groupID, Guid staffID, Guid managerCategoryID)
        {
            var chack = CheckGroup(groupID);
            if (!chack.Success)
            {
                return chack;
            }
            var service = new WorkGroupMemberService(this.Context);

            var manager = GetManager(service, groupID, managerCategoryID);
            if (manager != null)
            {
                return new OperateResult("已设置该类别的管理员,先取消再操作", false);
            }

            var member = GetStaff(service, groupID, staffID);
            if (member == null)
            {
                return new OperateResult("组内没有此员工", false);
            }

            service.Update(member.ID, new { CategoryID = managerCategoryID });
            return new OperateResult();
        }

        public OperateResult CancelManager(Guid groupID, Guid staffID, Guid managerCategoryID)
        {
            var chack = CheckGroup(groupID);
            if (!chack.Success)
            {
                return chack;
            }
            var service = new WorkGroupMemberService(this.Context);

            var member = GetStaff(service, groupID, staffID);
            if (member == null)
            {
                return new OperateResult("组内没有此员工", false);
            }

            var manager = GetManager(service, groupID, managerCategoryID);
            if (manager == null)
            {
                return new OperateResult("该组未设置该类别管理员", false);
            }

            service.Update(manager.ID, new { CategoryID = new Guid() });
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

        WorkGroupMember GetStaff(WorkGroupMemberService service, Guid groupID, Guid staffID)
        {
            return service.Single(p => p.WorkGroupID == groupID && p.StaffID == staffID);
        }

        WorkGroupMember GetManager(WorkGroupMemberService service, Guid groupID, Guid categoryID)
        {
            return service.Single(p => p.WorkGroupID == groupID && p.CategoryID == categoryID);
        }
    }
}
