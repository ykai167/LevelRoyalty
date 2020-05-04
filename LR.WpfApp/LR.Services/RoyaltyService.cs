using LR.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoyaltyService : IQueryService<Royalty>
    {
    }

    public class RoyaltyService : InsertServiceBase<Royalty>, IRoyaltyService
    {
        public RoyaltyService()
        {

        }

        public RoyaltyService(Repositories.DataContext context) : base(context)
        {

        }
    }
}
