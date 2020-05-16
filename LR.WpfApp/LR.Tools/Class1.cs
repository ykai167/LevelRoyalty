using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LR.Tools
{
    public static class NumberExtends
    {
        public static decimal Places(this decimal num, int place = 2)
        {
            return Math.Round(num * 1.0000m, place);
        }
    }
}
