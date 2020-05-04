using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IUpdateService<T> : IInsertService<T> where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        void Update(Guid id, object columData);
    }

    public partial class UpdateServiceBase<T> : InsertServiceBase<T>, IUpdateService<T> where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
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
