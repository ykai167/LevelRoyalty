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
    public class RoyaltySettleServiceTests : TestBase
    {
        [TestMethod()]
        public void SettlementTest()
        {
            var result = Tools.DIHelper.GetInstance<IRoyaltySettleService>().Settlement();
            Assert.IsTrue(result.Success);
        }
    }
}