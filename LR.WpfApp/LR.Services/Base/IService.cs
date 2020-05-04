using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public partial interface IService<T> : IDisposable where T : LR.Entity.IDEntity<Guid>, new()
    {
        List<T> All();
        T Single(Guid id);
        T Single(Expression<Func<T, bool>> exp);
        List<T> PageList(int pageIndex, int pageSize = 20);
        List<T> List();
        Guid Insert(T entity);
    }

    public interface IUpdateService<T> : IService<T> where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        void Update(Guid id, object columData);
    }

    public partial class ServiceBase<T> : IService<T> where T : LR.Entity.IDEntity<Guid>, new()
    {
        LR.Repositories.DataContext db;
        public ServiceBase()
        {
            db = new Repositories.DataContext();
        }

        protected LR.Repositories.DataContext Context { get { return this.db; } }

        protected virtual SqlSugar.ISugarQueryable<T> Queryable
        {
            get
            {
                return this.db.Context.Queryable<T>();
            }
        }

        public virtual Guid Insert(T entity)
        {
            if (entity.ID == new Guid())
            {
                entity.ID = Guid.NewGuid();
            }
            entity.CreateDate = DateTime.Now;
            db.Context.Insertable<T>(entity).ExecuteCommand();
            return entity.ID;
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

    public partial class UpdateServiceBase<T> : ServiceBase<T>, IUpdateService<T> where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        protected override ISugarQueryable<T> Queryable => base.Queryable.Where(p => p.State == LR.Entity.DataState.Normal);

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
            this.Context.Context.Updateable<T>(dic).Where(item => item.ID == id).ExecuteCommand();
        }

        public override Guid Insert(T entity)
        {
            entity.ModifyDate = DateTime.Now;
            entity.OperatorID = Administrator.Current.ID;
            entity.State = (int)DataState.Normal;
            base.Insert(entity);
            return entity.ID;
        }
    }
}
