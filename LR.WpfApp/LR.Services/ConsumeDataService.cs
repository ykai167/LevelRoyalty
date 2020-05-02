using LR.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IConsumeDataService : IService<ConsumeData>
    {

    }
    public class ConsumeDataService : ServiceBase<ConsumeData>, IConsumeDataService
    {
        public override void Insert(ConsumeData entity)
        {
            base.Insert(entity);


        }

        public override void Update(ConsumeData entity)
        {
            base.Update(entity);
        }
    }

    public interface IStaffService : IService<LR.Entity.Staff>
    {
    }

    public class StaffService : ServiceBase<LR.Entity.ConsumeData>, IConsumeDataService
    {
    }

    public interface IStaffLevelService : IService<LR.Entity.StaffLevel>
    {
    }

    public class StaffLevelService : ServiceBase<LR.Entity.StaffLevel>, IStaffLevelService
    {

    }
}
