﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public interface IRoomService : IUpdateService<LR.Entity.Room>
    {
        object[] GetAll();
    }
    class RoomService : UpdateServiceBase<LR.Entity.Room>, IRoomService
    {
        public object[] GetAll()
        {
            return this.Context.Context.Queryable<Entity.Room, Entity.RoomCategory>((r, rc) => r.CategoryID == rc.ID)
                .Where(r => r.State == LR.Entity.DataState.Normal)
                 .Select((r, rc) => new
                 {
                     r.ID,
                     r.Name,
                     r.CategoryID,
                     r.CreateDate,
                     r.ModifyDate,
                     r.No,
                     CategoryName = rc.Name
                 }).ToArray();
        }
    }
}
