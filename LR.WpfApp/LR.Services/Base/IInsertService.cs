﻿using System;
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
        public virtual Guid Insert(T entity)
        {
            if (entity.ID == new Guid())
            {
                entity.ID = Guid.NewGuid();
            }
            entity.CreateDate = DateTime.Now;
            this.Context.Context.Insertable<T>(entity).ExecuteCommand();
            return entity.ID;
        }
    }
}
