using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IWorkGroupManagerCategoryService : IService<LR.Entity.WorkGroupManagerCategory>
    {
    }

    public class WorkGroupManagerCategoryService : ServiceBase<LR.Entity.WorkGroupManagerCategory>, IWorkGroupManagerCategoryService
    {

    }
}
