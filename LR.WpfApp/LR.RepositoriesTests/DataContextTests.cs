using Microsoft.VisualStudio.TestTools.UnitTesting;
using LR.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Repositories.Tests
{
    [TestClass()]
    public class DataContextTests
    {
        [TestMethod()]
        public void DataContextTest()
        {
            var context = new LR.Repositories.DataContext();

            var props = context.GetType().GetProperties().Where(p => p.PropertyType.IsGenericType).ToArray();
            var arry = new object[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                var value = props[i].GetValue(context);
                arry[i] = value.GetType().GetMethod("GetList", new Type[0])?.Invoke(value, null);
            }

            Assert.IsTrue(arry.All(p => p != null));
        }
        [TestMethod()]
        public void StaffAddTest()
        {
            var context = new LR.Repositories.DataContext();
            var a = new LR.Entity.Staff
            {
                ID = Guid.NewGuid(),
                Name = "张三"
            };
            var a1 = new LR.Entity.Staff
            {
                ID = Guid.NewGuid(),
                Name = "张四",
                ReferrerID = a.ID
            }; var a2 = new LR.Entity.Staff
            {
                ID = Guid.NewGuid(),
                Name = "张五",
                ReferrerID = a.ID
            };

            var b = new LR.Entity.Staff
            {
                ID = Guid.NewGuid(),
                Name = "李四",
                ReferrerID = a1.ID
            };
            var b1 = new LR.Entity.Staff
            {
                ID = Guid.NewGuid(),
                Name = "李五",
                ReferrerID = b.ID
            };
            var b2 = new LR.Entity.Staff
            {
                ID = Guid.NewGuid(),
                Name = "李六",
                ReferrerID = b.ID
            };
            context.Staffs.InsertRange(new[] { a, a1, a2, b, b1, b2 }.Select(p =>
            {
                p.IdenNo = "";
                p.MobileNo = "";
                return p;
            }).ToArray());

            Assert.IsTrue(context.Staffs.Count(p => true) == 6);
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
                    Order = 0
                };
                var b = new LR.Entity.StaffLevel
                {
                    ID = Guid.NewGuid(),
                    Name = "中级",
                    MinCount = 2,
                    Order = 1
                };
                var c = new LR.Entity.StaffLevel
                {
                    ID = Guid.NewGuid(),
                    Name = "高级",
                    MinCount = 2,
                    Order = 2
                };

                context.StaffLevels.InsertRange(new[] { a, b, c });
            }


            Assert.IsTrue(context.StaffLevels.Count(p => true) == 3);
        }
    }
}