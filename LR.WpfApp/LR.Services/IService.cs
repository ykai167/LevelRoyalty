using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IService<T> where T : LR.Entity.IDEntity<Guid>
    {
        T Single(Guid id);
        List<T> PageList(int pageIndex, int pageSize = 20);
        void Update(T entity);
    }

    public class ServiceBase<T> : IService<T> where T : LR.Entity.IDEntity<Guid>
    {
        LR.Repositories.DataContext db;
        public ServiceBase()
        {
            db = new Repositories.DataContext();
        }

        public List<T> PageList(int pageIndex, int pageSize)
        {
            return db.Context.Queryable<T>().ToPageList(pageIndex, pageSize);
        }

        public T Single(Guid id)
        {

            return db.Context.Queryable<T>().Single(p => p.ID == id);
        }
        public T Single(Expression<Func<T, bool>> exp)
        {
            return db.Context.Queryable<T>().Single(exp);
        }

        public List<T> All()
        {
            return db.Context.Queryable<T>().ToList();
        }

        public void Update(T entity)
        {
            //db.Context.Updateable<T>(entity,);
        }
    }
}
