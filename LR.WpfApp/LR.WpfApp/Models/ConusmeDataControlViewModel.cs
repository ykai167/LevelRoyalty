using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.WpfApp.Models
{
    public class ConusmeDataControlViewModel : Prism.Mvvm.BindableBase
    {

    }

    public class CounsumeDataModel
    {
        public String StaffNo { get; set; }

        public String StaffName { get; set; }

        public String RoomNo { get; set; }

        public String RoomName { get; set; }

        public String Amount { get; set; }

        public String Admin { get; set; }

        public String CreateDate { get; set; }

        public String ModifyDate { get; set; }
    }
}
