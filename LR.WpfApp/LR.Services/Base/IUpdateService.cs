﻿using LR.Models;
using LR.Tools;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IUpdateService<T> : IInsertService<T> where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        void Update(Guid id, object columData);
        OperateResult Delete(Guid id);
    }

    public partial class UpdateServiceBase<T> : InsertServiceBase<T>, IUpdateService<T> where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        public UpdateServiceBase()
        {

        }
        public UpdateServiceBase(Repositories.DataContext context) : base(context)
        {

        }
        protected override ISugarQueryable<T> Queryable => base.Queryable.Where(p => p.State != LR.Entity.DataState.Delete);

        public virtual void Update(Guid id, object columData)
        {
            T t = null;
            Dictionary<string, object> dic = null;
            if (columData is Dictionary<string, object>)
            {
                dic = columData as Dictionary<string, object>;
            }
            else
            {
                dic = new Dictionary<string, object>();
                foreach (var prop in columData.GetType().GetProperties())
                {
                    dic[prop.Name] = prop.GetValue(columData);
                }
            }
            dic[nameof(t.ModifyDate)] = DateTime.Now;
            dic[nameof(t.OperatorID)] = Administrator.Current.ID;

            this.Context.Context.Insertable<Entity.Log>(new Entity.Log
            {
                Type = (int)LogType.Update,
                CreateDate = DateTime.Now,
                DataID = id,
                OperatorID = Administrator.Current.ID,
                ID = Guid.NewGuid(),
                Table = typeof(T).Name,
                Data = columData.LogJson()
            }).ExecuteCommand();
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

        public virtual OperateResult Delete(Guid id)
        {
            this.Update(id, new { State = Entity.DataState.Delete });
            return new OperateResult();
        }
    }

    internal interface IDeleteService<T> : IUpdateService<T> where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        void Delete(Expression<Func<T, bool>> expression);
    }

    partial class DeleteServiceBase<T> : UpdateServiceBase<T>, IDeleteService<T> where T : LR.Entity.UpdateEntity<Guid, Guid>, new()
    {
        public DeleteServiceBase()
        {

        }
        public DeleteServiceBase(Repositories.DataContext context) : base(context)
        {

        }

        public override OperateResult Delete(Guid id)
        {
            this.Context.Context.Deleteable<T>().Where(item => item.ID == id).ExecuteCommand();
            return new OperateResult();
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {
            this.Context.Context.Deleteable<T>().Where(expression).ExecuteCommand();
        }
    }
}
