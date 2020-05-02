using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public class MemoryData
    {
        public IEnumerable<Staff> Staffs;

        MemoryData()
        {
            this.ReloadStaffs();
        }
        public void ReloadStaffs()
        {
            var db = new LR.Repositories.DataContext();
            this.Staffs = db.Staffs.GetList(item => item.State == (int)StaffState.Normal).Select(item => new Staff
            {
                ID = item.ID,
                Name = item.Name,
                ParentID = item.ReferrerID
            }).ToArray();

            foreach (var item in this.Staffs)
            {
                item.Subs = this.Staffs.Where(p => p.ParentID == item.ID).ToArray();
            }
        }

        static MemoryData current = new MemoryData();
        public static MemoryData Current { get { return current; } }
    }

    public class Level
    {
        public readonly static Level Min;

        static Level()
        {
            var db = new LR.Repositories.DataContext();
            var list = db.StaffLevels.GetList().OrderBy(item => item.Order).Select(item => new Level
            {
                ID = item.ID,
                Name = item.Name,
                MinCount = item.MinCount,
                Order = item.Order
            }).ToArray();
            Level current = null;
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
        public Level Upper { get; set; }
        public override string ToString()
        {
            return this.Name;
        }
    }
    public class Staff
    {
        public string Name { get; set; }
        Level _level;
        public Level Level
        {
            get
            {
                if (this._level == null)
                {
                    if (Subs.Length == 0)
                    {
                        this._level = Level.Min;
                    }
                    else
                    {
                        var group = Subs.Select(item => item.Level)
                            .Distinct()
                            .Select(level => new
                            {
                                level,
                                items = Subs.Where(s => s.Level.Order >= level.Order)
                            }).Where(item => item.items.Count() >= item.level.MinCount)
                            .OrderByDescending(g => g.level.Order)
                            .FirstOrDefault();

                        var group2 = Subs.GroupBy(item => item.Level)
                            .Where(g => g.Count() >= g.Key.MinCount)
                            .OrderByDescending(g => g.Key.Order)
                            .FirstOrDefault();

                        this._level = group?.level.Upper ?? group?.level ?? Level.Min;
                    }
                }
                return this._level;
            }
        }
        //[Newtonsoft.Json.JsonIgnore]
        public Staff[] Subs { get; set; }
        public Guid ID { get; set; }
        public Guid ParentID { get; set; }
        public override string ToString()
        {
            return $"{ID}\t{Name}\t{Level}\t[{string.Join(",", Subs.AsEnumerable())}]";
        }
    }
}
