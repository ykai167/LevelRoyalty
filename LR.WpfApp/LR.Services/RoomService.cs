using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoomService : IService<LR.Entity.Room>
    {

    }
    public class RoomService : ServiceBase<LR.Entity.Room>, IRoomService
    {
    }
}
