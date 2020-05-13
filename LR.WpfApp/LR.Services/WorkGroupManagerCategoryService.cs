using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IWorkGroupManagerCategoryService : IUpdateService<LR.Entity.WorkGroupManagerCategory>
    {
    }

    class WorkGroupManagerCategoryService : UpdateServiceBase<LR.Entity.WorkGroupManagerCategory>, IWorkGroupManagerCategoryService
    {

    }
}
