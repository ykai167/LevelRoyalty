using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IWorkGroupService : IService<LR.Entity.WorkGroup>
    {
    }

    public class WorkGroupService : ServiceBase<LR.Entity.WorkGroup>, IWorkGroupService
    {
    }
}
