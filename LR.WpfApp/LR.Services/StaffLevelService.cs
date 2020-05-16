using LR.Entity;
using LR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IStaffLevelService : IUpdateService<LR.Entity.StaffLevel>
    {
    }

    class StaffLevelService : UpdateServiceBase<LR.Entity.StaffLevel>, IStaffLevelService
    {
        public override OperateResult Delete(Guid id)
        {
            var r = base.Delete(id);
            LevelModel.Load();
            return r;

        }
        public override Guid Insert(StaffLevel entity)
        {
            var g = base.Insert(entity);
            LevelModel.Load();
            return g;
        }
        public override void Update(Guid id, object columData)
        {
            base.Update(id, columData);
            LevelModel.Load();
        }
    }
}
