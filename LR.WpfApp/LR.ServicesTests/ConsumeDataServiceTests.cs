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
        public void AddDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertTest()
        {
            var service = Tools.DIHelper.GetInstance<IConsumeDataService>();

            service.Insert(new ConsumeData
            {
                StaffID = LR.Services.MemoryData.Current.Staffs.LastOrDefault(p => p.Level == LR.Models.LevelModel.Min).ID,
                Amount = 100
            });

            Assert.Fail();
        }
    }
}