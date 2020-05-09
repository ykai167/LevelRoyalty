using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Tools
{
    public static class ObjectExtends
    {
        public static object GetValue(this object obj, string name)
        {
            var p = obj.GetType().GetProperty(name);
            if (p != null)
            {
                return p.GetValue(obj);
            }
            return null;
        }
    }
}
