using LR.Entity;
using LR.Models;
using LR.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IAdminService : IUpdateService<LR.Entity.Admin>
    {
        Models.OperateResult Add(Admin entity);
        Models.OperateResult Login(string name, string ps);
        Models.OperateResult ChangePassword(string old, string @new);
        object[] GetList();
    }

    class AdminService : UpdateServiceBase<LR.Entity.Admin>, IAdminService
    {
        public OperateResult Add(Admin entity)
        {
            if (this.Context.Admins.IsAny(p => p.UserName == entity.UserName))
            {
                return new OperateResult("登录名重复", false);
            }


            entity.Password = Administrator.PsString(entity.Password);
            entity.Type = (int)AdminType.Ordin;
            base.Insert(entity);
            return new OperateResult();
        }
        public OperateResult ChangePassword(string old, string @new)
        {
            var admin = this.Single(p => p.ID == Administrator.Current.ID);
            if (admin == null)
            {
                return new OperateResult("测试模式,不可修改", false);
            }
            if (admin.Password != Administrator.PsString(old))
            {
                return new OperateResult("原密码错误", false);
            }
            if (@new.Length < 6)
            {
                return new OperateResult("密码长度至少6位", false);
            }
            this.Update(admin.ID, new { Password = Administrator.PsString(@new) });
            return new OperateResult();
        }

        public OperateResult Login(string name, string ps)
        {
            var admin = this.Single(p => p.UserName == name);
            if (admin == null)
            {
                return new OperateResult("用户名不存在", false) { };
            }
            else if (Administrator.PsString(ps) != admin.Password)
            {
                return new OperateResult("密码错误", false);
            }
            else if (admin.State == (int)AdminState.Disable)
            {
                return new OperateResult("用户已禁用,请联系超级管理员");
            }
            Administrator.Current = new Administrator
            {
                ID = admin.ID,
                Name = admin.Name,
                Type = (AdminType)admin.Type
            };
            this.Context.Context.Insertable<Entity.Log>(new Entity.Log
            {
                Type = (int)LogType.Login,
                CreateDate = DateTime.Now,
                DataID = Guid.Empty,
                OperatorID = Administrator.Current.ID,
                ID = Guid.NewGuid(),
                Table = "",
                Data = new { }.LogJson()
            }).ExecuteCommand();
            return new OperateResult();
        }

        public object[] GetList()
        {
            return this.Queryable.Where(p => p.Type == (int)AdminType.Ordin)
                .ToArray()
                .Select(p => new
                {
                    p.ID,
                    p.Name,
                    p.UserName,
                    p.State,
                    p.CreateDate,
                    p.ModifyDate,
                    StateName = ((AdminState)p.State).GetName()
                }).ToArray();
        }
    }
}
