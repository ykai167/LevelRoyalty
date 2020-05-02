﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public partial interface IService<T> where T : LR.Entity.IDEntity<Guid>
    {
        T Single(Guid id);
        List<T> PageList(int pageIndex, int pageSize = 20);
        void Update(T entity);
        void Insert(T entity);
    }

    public partial class ServiceBase<T> : IService<T> where T : LR.Entity.IDEntity<Guid>, new()
    {
        LR.Repositories.DataContext db;
        public ServiceBase()
        {
            db = new Repositories.DataContext();
        }

        public void Insert(T entity)
        {
            db.Context.Insertable<T>(entity).ExecuteCommand();
        }

        public List<T> PageList(int pageIndex, int pageSize)
        {
            return db.Context.Queryable<T>().ToPageList(pageIndex, pageSize);
        }

        public T Single(Guid id)
        {
            return db.Context.Queryable<T>().Single(p => p.ID == id);
        }

        public void Update(T entity)
        {
            db.Context.Updateable<T>(entity).Where(item => item.ID == entity.ID).ExecuteCommand();
        }
    }
}
