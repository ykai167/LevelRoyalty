using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public partial interface IService<T> where T : LR.Entity.IDEntity<Guid>
    {

    }
    public partial class ServiceBase<T>
    {

    }
}
