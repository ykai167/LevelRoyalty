using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LR.Tools
{
    public class Pager<T> : IEnumerable<T>
    {
        IEnumerable<T> data;
        int total;

        public Pager(IEnumerable<T> data, int pageSize, int total)
        {
            this.data = data.ToArray();
            this.total = total;
            this.PageSize = pageSize;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int PageSize { get; }
        public int TotalPage { get { return total % PageSize == 0 ? total / PageSize : total / PageSize + 1; } }
    }
}
