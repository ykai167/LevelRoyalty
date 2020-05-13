using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoomCategoryService : IUpdateService<LR.Entity.RoomCategory>
    {

    }
    class RoomCategoryService : UpdateServiceBase<LR.Entity.RoomCategory>, IRoomCategoryService
    {

    }
}
