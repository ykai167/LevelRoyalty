using Microsoft.VisualStudio.TestTools.UnitTesting;
using LR.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR.Entity;

namespace LR.Repositories.Tests
{
    [TestClass()]
    public partial class DataContextTests
    {

        [TestMethod()]
        public void RoomAddTest()
        {
            var context = new LR.Repositories.DataContext(true);
            var roomcategory = new RoomCategory
            {
                ID = Guid.NewGuid(),
                Name = "一等",
                State = LR.Entity.DataState.Normal,
                MinCharge = 200
            };
            context.RoomCategories.Insert(roomcategory);

            context.Rooms.Insert(new Room
            {
                CategoryID = roomcategory.ID,
                ID = Guid.NewGuid(),
                No = "001",
                Name = "歌厅",
                State = DataState.Normal,
                Summary = "ceshi"
            });
            Assert.IsTrue(context.Rooms.Count(p => true) > 0);
        }

        [TestMethod()]
        public void WorkgroyManagerCategoryAddTest()
        {
            var context = new LR.Repositories.DataContext(true);
            if (context.WorkGroupManagerCategories.Count(p => p.State == 200) == 0)
            {
                context.WorkGroupManagerCategories.Insert(new Entity.WorkGroupManagerCategory
                {
                    ID = Guid.NewGuid(),
                    Name = "妈咪",
                    State = Entity.DataState.Normal
                });
                context.WorkGroupManagerCategories.Insert(new Entity.WorkGroupManagerCategory
                {
                    ID = Guid.NewGuid(),
                    Name = "助理",
                    State = Entity.DataState.Normal
                });
            }

            Assert.IsTrue(context.WorkGroupManagerCategories.Count(p => p.State == 200) == 2);
        }



        [TestMethod()]
        public void StaffAddTest()
        {
            var context = new LR.Repositories.DataContext();

            var list = new List<LR.Entity.Staff>();

            string name = "李四";
            int index = 0;
            Random r = new Random();
            LR.Entity.Staff staff = null;
            for (int i = 0; i < 10; i++)
            {
                staff = new Entity.Staff
                {
                    ID = Guid.NewGuid(),
                    Name = $"{name}{index++}",
                    ReferrerID = staff?.ID ?? new Guid()
                };
                list.Add(staff);
                list.Add(new Entity.Staff
                {
                    ID = Guid.NewGuid(),
                    Name = $"{name}{index++}",
                    ReferrerID = staff.ID
                });
                if (r.Next(0, 2) == 1)
                {
                    list.Add(staff = new Entity.Staff
                    {
                        ID = Guid.NewGuid(),
                        Name = $"{name}{index++}",
                        ReferrerID = staff.ID
                    });
                }
            }


            context.Staffs.InsertRange(list.Select(p =>
            {
                p.IdenNo = "";
                p.MobileNo = "";
                p.State = Entity.DataState.Normal;
                return p;
            }).ToArray());

            Assert.IsTrue(context.Staffs.Count(p => true) > 0);
        }
        [TestMethod()]
        public void StaffLevelInitTest()
        {
            var context = new LR.Repositories.DataContext();
            if (context.StaffLevels.Count(t => true) == 0)
            {
                var a = new LR.Entity.StaffLevel
                {
                    ID = Guid.NewGuid(),
                    Name = "初级",
                    MinCount = 2,
                    Order = 0,
                    State = LR.Entity.DataState.Normal
                };
                var b = new LR.Entity.StaffLevel
                {
                    ID = Guid.NewGuid(),
                    Name = "中级",
                    MinCount = 2,
                    Order = 1,
                    State = LR.Entity.DataState.Normal
                };
                var c = new LR.Entity.StaffLevel
                {
                    ID = Guid.NewGuid(),
                    Name = "高级",
                    MinCount = 2,
                    Order = 2,
                    State = LR.Entity.DataState.Normal
                };

                context.StaffLevels.InsertRange(new[] { a, b, c });
            }


            Assert.IsTrue(context.StaffLevels.Count(p => true) == 3);
        }


    }
}