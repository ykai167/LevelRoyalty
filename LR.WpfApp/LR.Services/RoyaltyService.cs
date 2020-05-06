using LR.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoyaltyService : IUpdateService<Royalty>
    {
    }

    public class RoyaltyService : UpdateServiceBase<Royalty>, IRoyaltyService
    {
        public RoyaltyService()
        {

        }

        public RoyaltyService(Repositories.DataContext context) : base(context)
        {

        }
    }
}
