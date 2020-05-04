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

        [TestMethod()]
        public void DataContextTranTest()
        {
            using (var context1 = new LR.Repositories.DataContext())
            using (var context2 = new LR.Repositories.DataContext())
            {
                try
                {
                    context1.Context.Ado.BeginTran();
                    context2.ConsumeDatas.Delete(p => true);
                    context2.ConsumeDatas.Insert(new Entity.ConsumeData
                    {
                        Amount = 100,
                        ID = Guid.NewGuid(),
                        RoomID = Guid.NewGuid(),
                        StaffID = Guid.NewGuid(),
                        State = 100
                    });
                    context2.ConsumeDatas.Insert(new Entity.ConsumeData
                    {
                        Amount = 100,
                        ID = Guid.NewGuid(),
                        RoomID = Guid.NewGuid(),
                        StaffID = Guid.NewGuid(),
                        State = 100
                    });
                    context1.Context.Ado.CommitTran();
                }
                catch (Exception e)
                {
                    context1.Context.Ado.RollbackTran();
                    throw e;
                }
                finally
                {
                    Assert.IsTrue(context1.ConsumeDatas.Count(p => true) == 2);
                }
            }
        }

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
    }
}