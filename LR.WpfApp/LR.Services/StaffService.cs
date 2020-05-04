using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IStaffService : IUpdateService<LR.Entity.Staff>
    {
    }

    public class StaffService : UpdateServiceBase<LR.Entity.Staff>, IStaffService
    {
    }
}
