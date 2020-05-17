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
    public class MemoryDataTests : TestBase
    {
        enum Te
        {
            NN = 100
        }

        [TestMethod()]
        public void ReloadStaffsTest()
        {
            var aa = Enum.Parse(typeof(Te), (100).ToString());
            var mamery = LR.Services.MemoryData.Current;
            Assert.IsNotNull(mamery);
        }

        [TestMethod()]
        public void LogHelperTest()
        {

            var mamery = LR.Services.LogHelper.Current;
            Assert.IsNotNull(mamery);
        }

    }
}