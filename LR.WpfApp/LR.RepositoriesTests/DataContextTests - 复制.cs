using Microsoft.VisualStudio.TestTools.UnitTesting;
using LR.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Repositories.Tests
{
    public partial class DataContextTests
    {
        //3486a760-dff8-4596-9764-97dedd9f0e48

        [TestMethod()]
        public void UpdateTest()
        {
            using (var context = new LR.Repositories.DataContext())
            {
                var list = context.RoyaltyConfigs.GetList();
                var id = Guid.Parse("3486a760-dff8-4596-9764-97dedd9f0e48");
                var old = context.RoyaltyConfigs.GetSingle(p => p.ID == id);

                context.Context.Updateable<Entity.RoyaltyConfig>(item => new Entity.RoyaltyConfig
                {
                    Percent = 1.55m,
                    ModifyDate = DateTime.Now
                }).Where(p => p.ID == old.ID).ExecuteCommand();

                var news = context.RoyaltyConfigs.GetSingle(p => p.ID == id);
                Assert.IsTrue(news.Percent == 1.55m && old.CreateDate == news.CreateDate);
            }
        }
        [TestMethod()]
        public void UpdateByDicTest()
        {
            using (var context = new LR.Repositories.DataContext())
            {
                var list = context.RoyaltyConfigs.GetList();
                var id = Guid.Parse("3486a760-dff8-4596-9764-97dedd9f0e48");
                var old = context.RoyaltyConfigs.GetSingle(p => p.ID == id);

                context.Context.Updateable<Entity.RoyaltyConfig>(new Dictionary<string, object>
                {
                    ["Percent"] = 13.66m,
                    ["ModifyDate"] = DateTime.Now
                }).Where(p => p.ID == old.ID).ExecuteCommand();

                var news = context.RoyaltyConfigs.GetSingle(p => p.ID == id);
                Assert.IsTrue(news.Percent == 13.66m && old.CreateDate == news.CreateDate);
            }
        }

    }
}