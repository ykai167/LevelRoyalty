using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IStaffService :　IService<LR.Entity.Staff>
    {
    }

    public class StaffService : ServiceBase<LR.Entity.Staff>, IStaffService
    {
    }    
}
