using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public partial interface IService<T> : IDisposable where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        List<T> All();
        T Single(Guid id);
        T Single(Expression<Func<T, bool>> exp);
        List<T> PageList(int pageIndex, int pageSize = 20);
        List<T> List();
        void Update(Guid id, object columData);
        void Insert(T entity);
    }

    public partial class ServiceBase<T> : IService<T> where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        LR.Repositories.DataContext db;
        public ServiceBase()
        {
            db = new Repositories.DataContext();
        }

        protected LR.Repositories.DataContext Context { get { return this.db; } }

        protected SqlSugar.ISugarQueryable<T> Queryable
        {
            get
            {
                return this.db.Context.Queryable<T>().Where(item => item.State == (int)DataState.Normal);
            }
        }

        public virtual void Insert(T entity)
        {
            entity.ModifyDate = entity.CreateDate = DateTime.Now;
            entity.OperatorID = Administrator.Current.ID;
            db.Context.Insertable<T>(entity).ExecuteCommand();
        }

        public virtual List<T> List()
        {
            return this.Queryable.ToList();
        }

        public virtual List<T> PageList(int pageIndex, int pageSize)
        {
            return this.Queryable.ToPageList(pageIndex, pageSize);
        }

        public T Single(Guid id)
        {
            return this.Queryable.Single(p => p.ID == id);
        }

        public virtual void Update(Guid id, object columData)
        {
            T t = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic[nameof(t.ModifyDate)] = DateTime.Now;
            dic[nameof(t.OperatorID)] = Administrator.Current.ID;
            foreach (var prop in columData.GetType().GetProperties())
            {
                dic[prop.Name] = prop.GetValue(columData);
            }
            db.Context.Updateable<T>(dic).Where(item => item.ID == id).ExecuteCommand();
        }
        public T Single(Expression<Func<T, bool>> exp)
        {
            return db.Context.Queryable<T>().Single(exp);
        }

        public List<T> All()
        {
            return db.Context.Queryable<T>().ToList();
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
