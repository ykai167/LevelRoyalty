using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Tools
{
    public static class ObjectExtends
    {
        public static object GetObjectValue(this object obj, string name)
        {
            if (obj == null)
            {

            }
            else
            {
                var p = obj.GetType().GetProperty(name);
                if (p != null)
                {
                    return p.GetValue(obj);
                }
            }

            return null;
        }
        public static T GetObjectValue<T>(this object obj, string name)
        {
            var p = obj.GetType().GetProperty(name);
            if (p != null)
            {
                try
                {
                    return (T)p.GetValue(obj);
                }
                catch
                {
                }
            }
            return default(T);
        }
    }
}
