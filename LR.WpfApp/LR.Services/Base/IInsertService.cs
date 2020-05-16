using LR.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public partial interface IInsertService<T> : IQueryService<T> where T : LR.Entity.IDEntity<Guid>, new()
    {
        Guid Insert(T entity);
    }

    public partial class InsertServiceBase<T> : QueryServiceBase<T>, IInsertService<T> where T : LR.Entity.IDEntity<Guid>, new()
    {
        public InsertServiceBase()
        {

        }
        public InsertServiceBase(Repositories.DataContext context) : base(context)
        {

        }
        public virtual Guid Insert(T entity)
        {
            if (entity.ID == new Guid())
            {
                entity.ID = Guid.NewGuid();
            }
            entity.CreateDate = DateTime.Now;
            this.Context.Context.Insertable<T>(entity).ExecuteCommand();
            this.Context.Context.Insertable<LR.Entity.Log>(new Entity.Log
            {
                CreateDate = DateTime.Now,
                ID = Guid.NewGuid(),
                OperatorID = LR.Services.Administrator.Current.ID,
                Table = typeof(T).Name,
                Data = entity.LogJson(),
                Type = (int)LogType.Insert,
                DataID = entity.ID
            }).ExecuteCommand();
            return entity.ID;
        }
    }
}
