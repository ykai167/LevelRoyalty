using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LR.Models;

namespace LR.Services
{
    public static class Extends
    {
        public static string GetName(this Enum e)
        {
            var attr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(EnumNameAttribute), false).FirstOrDefault() as EnumNameAttribute;
            return attr?.Name;
        }

        public static LevelModel[] Downer(this LevelModel level)
        {
            var current = LevelModel.Min;
            List<LevelModel> list = new List<LevelModel>(); ;
            while (current != null && current.ID != level.ID)
            {
                list.Add(current);
                current = current.Upper;
            }
            return list.ToArray();
        }
    }
}
