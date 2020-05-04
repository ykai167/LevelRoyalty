using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoomService : IUpdateService<LR.Entity.Room>
    {

    }
    public class RoomService : UpdateServiceBase<LR.Entity.Room>, IRoomService
    {
    }
}
