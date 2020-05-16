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
        Models.OperateResult Login(string name, string ps);
        Models.OperateResult ChangePassword(string old, string @new);
    }

    class AdminService : UpdateServiceBase<LR.Entity.Admin>, IAdminService
    {
        public OperateResult ChangePassword(string old, string @new)
        {
            var admin = this.Single(p => p.ID == Administrator.Current.ID);
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
            Administrator.Current = new Administrator
            {
                ID = admin.ID,
                Name = admin.UserName,
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
    }
}
