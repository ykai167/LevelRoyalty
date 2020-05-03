using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IStaffLevelService : IService<LR.Entity.StaffLevel>
    {
    }

    public class StaffLevelService : ServiceBase<LR.Entity.StaffLevel>, IStaffLevelService
    {

    }
}
