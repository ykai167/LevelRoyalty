using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public partial interface IService<T> : IDisposable where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        T Single(Guid id);
        List<T> PageList(int pageIndex, int pageSize = 20);
        List<T> List();
        void Update(T entity);
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

        public virtual void Update(T entity)
        {
            entity.ModifyDate = DateTime.Now;
            entity.OperatorID = Administrator.Current.ID;
            db.Context.Updateable<T>(entity).Where(item => item.ID == entity.ID).ExecuteCommand();
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
