using Microsoft.VisualStudio.TestTools.UnitTesting;
using LR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services.Tests
{
    [TestClass()]
    public class MemoryDataTests
    {
        [TestMethod()]
        public void ReloadStaffsTest()
        {

            var mamery = LR.Services.MemoryData.Current;
            Assert.Fail();
        }
    }
}