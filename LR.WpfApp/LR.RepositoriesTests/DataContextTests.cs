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
    }
}