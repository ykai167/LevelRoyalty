using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IAdminService : IUpdateService<LR.Entity.Admin>
    {
        int Check(LR.Entity.Admin admin);
    }

    public class AdminService : UpdateServiceBase<LR.Entity.Admin>, IAdminService
    {
        public int Check(LR.Entity.Admin admin)
        {
            return -1;
        }

    }
}
