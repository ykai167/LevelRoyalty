using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface ISettleBatchService : IService<LR.Entity.SettleBatch>
    {
    }

    public class SettleBatchService : ServiceBase<LR.Entity.SettleBatch>, ISettleBatchService
    {
    }
}
