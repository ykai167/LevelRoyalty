using Microsoft.VisualStudio.TestTools.UnitTesting;
using LR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR.Entity;

namespace LR.Services.Tests
{
    public class TestBase
    {
        public TestBase()
        {
            var baseType = typeof(LR.Services.IQueryService<>);
            var types = baseType.Assembly.GetTypes()
                .Where(t => !t.IsInterface && !t.IsGenericType && t.GetInterfaces().Any(p => p.Name == baseType.Name));
            foreach (var type in types)
            {
                Tools.DIHelper.RegistTransient(type.GetInterfaces().Last(), type);
            }

            LR.Services.Administrator.Current = new Administrator();
        }
    }
    [TestClass()]
    public class ConsumeDataServiceTests : TestBase
    {

        [TestMethod()]
        public void AddWorkGroupDataTest()
        {
            var service = Tools.DIHelper.GetInstance<IWorkGroupService>();

            var old = service.List();

            var mcategory = Tools.DIHelper.GetInstance<IWorkGroupManagerCategoryService>().List();
            var newID = service.Insert(new WorkGroup { Name = "青年组" });

            var staffs = Tools.DIHelper.GetInstance<IStaffService>().List();
            for (int i = 0; i < staffs.Count; i++)
            {
                if (i % 2 == 0)
                {
                    service.AddMember(newID, staffs[i].ID);
                }
                if (i % 3 < mcategory.Count)
                {
                    service.SetManager(newID, staffs[i].ID, mcategory[i % 3].ID);
                }
            }

            var currenty = service.List();

            Assert.IsTrue(currenty.Count - old.Count == 1);
        }

        [TestMethod()]
        public void InsertTest()
        {
            var service = Tools.DIHelper.GetInstance<IConsumeDataService>();
            var old = service.List().Count();
            var room = Tools.DIHelper.GetInstance<IRoomService>().List().LastOrDefault();
            for (int i = 0; i < 10; i++)
            {
                var list = Tools.DIHelper.GetInstance<WorkGroupMemberService>().List();
                if (list.Count > 0)
                {
                    service.Insert(new ConsumeData
                    {
                        StaffID = list.FirstOrDefault(p => p.CategoryID != new Guid()).StaffID,
                        Amount = new Random().Next(100, 500),
                        RoomID = room.ID
                    });
                    service.Insert(new ConsumeData
                    {
                        StaffID = list[new Random().Next(0, list.Count > 3 ? 3 : list.Count)].StaffID,
                        Amount = new Random().Next(100, 500),
                        RoomID = room.ID
                    });
                }
            }


            Assert.IsTrue(service.List().Count() > old);
        }
    }
}