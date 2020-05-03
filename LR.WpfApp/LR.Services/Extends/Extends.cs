using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public static class Extends
    {
        public static string GetName(this Enum e)
        {
            var attr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(EnumNameAttribute), false).FirstOrDefault() as EnumNameAttribute;
            return attr?.Name;
        }

        public static Level[] Downer(this Level level)
        {
            var current = Level.Min;
            List<Level> list = new List<Level>(); ;
            while (current != null && current.ID != level.ID)
            {
                list.Add(current);
                current = current.Upper;
            }
            return list.ToArray();
        }
    }
}
