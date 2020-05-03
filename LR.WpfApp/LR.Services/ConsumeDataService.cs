using LR.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IConsumeDataService : IService<ConsumeData>
    {

    }
    public class ConsumeDataService : ServiceBase<ConsumeData>, IConsumeDataService
    {
        public override void Insert(ConsumeData entity)
        {
            //增加消费记录
            //
            base.Insert(entity);
        }
    }  
}
