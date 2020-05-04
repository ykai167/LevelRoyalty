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
    [TestClass()]
    public class ConsumeDataServiceTests
    {
        public ConsumeDataServiceTests()
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
        [TestMethod()]
        public void AddWorkGroupDataTest()
        {
            var service = Tools.DIHelper.GetInstance<IWorkGroupService>();

            var old = service.All();

            var mcategory = Tools.DIHelper.GetInstance<IWorkGroupManagerCategoryService>().All();
            var newID = service.Insert(new WorkGroup { Name = "青年组" });

            var staffs = Tools.DIHelper.GetInstance<IStaffService>().All();
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

            var currenty = service.All();

            Assert.IsTrue(currenty.Count - old.Count == 1);
        }

        [TestMethod()]
        public void InsertTest()
        {
            var service = Tools.DIHelper.GetInstance<IConsumeDataService>();

            service.Insert(new ConsumeData
            {
                StaffID = Guid.Parse("a5c5ae5f-7b56-4516-8ce3-a7d503de9755"),
                Amount = 100
            });

            Assert.Fail();
        }
    }
}