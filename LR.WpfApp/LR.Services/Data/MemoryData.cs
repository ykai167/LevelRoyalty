using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LR.Models;

namespace LR.Services
{
    public class MemoryData
    {
        public IEnumerable<StaffModel> Staffs;

        public

        MemoryData()
        {
            this.ReloadStaffs();
        }
        public void ReloadStaffs()
        {
            var db = new LR.Repositories.DataContext();
            this.Staffs = db.Staffs.GetList(item => item.State == (int)StaffState.Normal).Select(item => new StaffModel
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
}
