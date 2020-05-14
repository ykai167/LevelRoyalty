using LR.Entity;
using LR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.WpfApp.Models
{
    public class CurrentSettleControlViewModel : Prism.Mvvm.BindableBase
    {
        IRoyaltyService _royaltyService;
        ISettleBatchService _settleBatchService;
        public CurrentSettleControlViewModel(IRoyaltyService royaltySettleService, ISettleBatchService settleBatchService)
        {
            this._royaltyService = royaltySettleService;
            this._settleBatchService = settleBatchService;
            var current = settleBatchService.GetOrGenCurrent();
            this.batchs = new SettleBatch { Num = current.Num, BeginEnd = $"{current.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}" };
            this.Rows = _royaltyService.Statistics(current.Num)
                .Select(item => new RoyaltySettleModel
                {
                    ID = item.ID,
                    StaffID = item.StaffID,
                    StaffNo = item.StaffNo,
                    StaffName = MemoryData.Current.Staffs.FirstOrDefault(p => p.ID == item.StaffID)?.Name,
                    Administration = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Administration).Value,
                    Cooperation = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Cooperation).Value,
                    Reservation = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Reservation).Value,
                    Transcend = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Transcend).Value,
                    WorkGroup = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.WorkGroup).Value,
                    Total = item.Total
                }).ToList();
            this.BatchSelectedCommand = new Prism.Commands.DelegateCommand<object>(BatchSelected);
        }

        Prism.Commands.DelegateCommand<object> BatchSelectedCommand;

        private SettleBatch batchs;

        public SettleBatch Batch
        {
            get { return batchs; }
            set { batchs = value; }
        }

        public List<RoyaltySettleModel> Rows { get; set; }
        void BatchSelected(object num)
        {
        }
        private List<object> detailes;

        public List<object> Detailes
        {
            get { return detailes; }
            set { detailes = value; base.RaisePropertyChanged(); }
        }

        public void ChangeStaff(Guid staffID)
        {
            this.Detailes = _royaltyService.Detaile(staffID, Batch.Num);
        }
    }
    public class RoyaltySettleModel
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 提成员工ID
        /// </summary>
        public Guid StaffID { get; set; }
        public string StaffNo { get; set; }
        /// <summary>
        /// 提成员工
        /// </summary>
        public string StaffName { get; set; }

        public decimal Reservation { get; set; }

        public decimal Administration { get; set; }

        public decimal Cooperation { get; set; }

        public decimal Transcend { get; set; }

        public decimal WorkGroup { get; set; }

        public decimal Total { get; set; }
    }

}
