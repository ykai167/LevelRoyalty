using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Entity
{
    public class IDEntity<TID>
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public TID ID { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
