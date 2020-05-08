using LR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.WpfApp.Models
{
    public class RoyaltySettleControlViewModel : Prism.Mvvm.BindableBase
    {
        IRoyaltySettleService _royaltySettleService; ISettleBatchService _settleBatchService;
        public RoyaltySettleControlViewModel(IRoyaltySettleService royaltySettleService, ISettleBatchService settleBatchService)
        {
            this._royaltySettleService = royaltySettleService;
            this._settleBatchService = settleBatchService;
            this.batchs = settleBatchService.List().Where(p => p.IsHistory).OrderByDescending(item => item.CreateDate).Select(item => new SettleBatch
            {
                Num = item.Num,
                BeginEnd = $"{item.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}至{item.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}"
            }).ToArray();
            var num = this.batchs.FirstOrDefault()?.Num ?? 0;
            this.Rows = _royaltySettleService.GetSettles(num)
                .Select(item => new RoyaltySettleModel
                {
                    StaffID = item.StaffID,
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

        private SettleBatch[] batchs;

        public SettleBatch[] Batchs
        {
            get { return batchs; }
            set { batchs = value; }
        }

        public List<RoyaltySettleModel> Rows { get; set; }
        void BatchSelected(object num)
        {
        }
    }

    public class SettleBatch
    {
        public int Num { get; set; }
        public string BeginEnd { get; set; }
    }

    public class RoyaltySettleModel
    {
        /// <summary>
        /// 提成员工ID
        /// </summary>
        public Guid StaffID { get; set; }

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
