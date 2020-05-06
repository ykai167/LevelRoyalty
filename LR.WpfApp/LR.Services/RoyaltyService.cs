using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoyaltyService : IService<LR.Entity.Royalty>
    {

    }
    public class RoyaltyService : ServiceBase<LR.Entity.Royalty>, IRoyaltyService
    {
    }
}
