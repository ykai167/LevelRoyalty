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

    public class StaffLevelService : UpdateServiceBase<LR.Entity.StaffLevel>, IStaffLevelService
    {

    }
}
