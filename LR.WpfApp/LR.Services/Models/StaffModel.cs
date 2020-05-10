using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    public class StaffModel
    {
        public string Name { get; set; }
        LevelModel _level;
        public LevelModel Level
        {
            get
            {
                if (this._level == null)
                {
                    if (Subs == null || Subs.Length == 0)
                    {
                        this._level = LevelModel.Min;
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

                        this._level = group?.level.Upper ?? group?.level ?? LevelModel.Min;
                    }
                }
                return this._level;
            }
        }
        //[Newtonsoft.Json.JsonIgnore]
        public StaffModel[] Subs { get; set; }
        public Guid ID { get; set; }
        public Guid ReferrerID { get; set; }
        public StaffModel Referrer { get; set; }
        public override string ToString()
        {
            return $"{ID}\t{Name}\t{Level}\t[{string.Join(",", Subs.AsEnumerable())}]";
        }
    }
}
