using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IStaffService : IUpdateService<LR.Entity.Staff>
    {
        object[] GetStaffs();
    }

    public class StaffService : UpdateServiceBase<LR.Entity.Staff>, IStaffService
    {
        public object[] GetStaffs()
        {
            //return this.Context.Context.Queryable<LR.Entity.Staff,>
            throw new Exception();
        }
    }
}
