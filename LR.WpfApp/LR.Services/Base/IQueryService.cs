using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public partial interface IQueryService<T> : IDisposable where T : LR.Entity.IDEntity<Guid>, new()
    {
        List<T> List();
        T Single(Guid id);
        T Single(Expression<Func<T, bool>> exp);
        List<T> PageList(int pageIndex, int pageSize = 20);
    }

    public partial class QueryServiceBase<T> : IQueryService<T> where T : LR.Entity.IDEntity<Guid>, new()
    {
        LR.Repositories.DataContext db;
        public QueryServiceBase()
        {
            db = new Repositories.DataContext();
        }

        public QueryServiceBase(LR.Repositories.DataContext context)
        {
            this.db = context;
        }

        protected LR.Repositories.DataContext Context { get { return this.db; } }

        protected virtual SqlSugar.ISugarQueryable<T> Queryable
        {
            get
            {
                return this.db.Context.Queryable<T>();
            }
        }

        public virtual List<T> PageList(int pageIndex, int pageSize)
        {
            return this.Queryable.ToPageList(pageIndex, pageSize);
        }

        public T Single(Guid id)
        {
            return this.Queryable.Single(p => p.ID == id);
        }


        public T Single(Expression<Func<T, bool>> exp)
        {
            return db.Context.Queryable<T>().Single(exp);
        }

        public List<T> List()
        {
            return this.Queryable.ToList();
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
