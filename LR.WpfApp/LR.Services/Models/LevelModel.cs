using LR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    public class LevelModel
    {
        public readonly static LevelModel Min;

        static LevelModel()
        {
            var list = LR.Tools.DIHelper.GetInstance<IStaffLevelService>().All().OrderBy(item => item.Order).Select(item => new LevelModel
            {
                ID = item.ID,
                Name = item.Name,
                MinCount = item.MinCount,
                Order = item.Order
            }).ToArray();
            LevelModel current = null;
            for (int i = 0; i < list.Length; i++)
            {
                current = list[i];
                if (list.Length > i + 1)
                {
                    current.Upper = list[i + 1];
                }
                if (i == 0)
                {
                    Min = current;
                }
            }
        }

        public Guid ID { get; set; }
        /// <summary>
        /// 等级名称
        /// </summary>
        public string Name { get; set; }
        public int Order { set; get; }
        /// <summary>
        /// 下级最小数量
        /// </summary>
        public int MinCount { get; set; }
        public LevelModel Upper { get; set; }
        public override string ToString()
        {
            return this.Name;
        }

        public static bool operator >=(LevelModel b, LevelModel c)
        {
            return b.Order >= c.Order;
        }
        public static bool operator <=(LevelModel b, LevelModel c)
        {
            return b.Order <= c.Order;
        }
    }
}
