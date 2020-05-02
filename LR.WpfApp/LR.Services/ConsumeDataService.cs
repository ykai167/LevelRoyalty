using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IConsumeDataService : IService<LR.Entity.ConsumeData>
    {

    }
    public class ConsumeDataService : ServiceBase<LR.Entity.ConsumeData>, IConsumeDataService
    {
        
    }

    public interface IStaffService : IService<LR.Entity.Staff>
    {
    }

    public class StaffService : ServiceBase<LR.Entity.ConsumeData>, IConsumeDataService
    {
    }
}
