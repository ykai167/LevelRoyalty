using LR.Entity;
using LR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR.Tools;

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
            this.Reload();
            this.BatchSelectedCommand = new Prism.Commands.DelegateCommand<object>(BatchSelected);
        }

        Prism.Commands.DelegateCommand<object> BatchSelectedCommand;

        private SettleBatch batchs;

        public SettleBatch Batch
        {
            get { return batchs; }
            set { batchs = value; base.RaisePropertyChanged(); }
        }

        List<RoyaltySettleModel> rows;
        public List<RoyaltySettleModel> Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
                base.RaisePropertyChanged();
                base.RaisePropertyChanged(nameof(this.CanSettlement));
            }
        }

        public bool CanSettlement { get { return this.Rows.Count > 0; } }
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
        public decimal AllTotal { get { return this.rows.Sum(p => p.Total).Places(); } }

        public void Reload()
        {
            var current = _settleBatchService.GetOrGenCurrent();
            this.Batch = new SettleBatch
            {
                Num = current.Num,
                BeginEnd = $"{current.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}"
            };
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
                }).ToList();
            base.RaisePropertyChanged(nameof(AllTotal));
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

        decimal _eservation;
        public decimal Reservation { get { return _eservation.Places(); } set { _eservation = value; } }
        decimal a;
        public decimal Administration { get { return a.Places(); } set { a = value; } }
        decimal c;
        public decimal Cooperation { get { return c.Places(); } set { c = value; } }
        decimal tr;
        public decimal Transcend { get { return tr.Places(); } set { tr = value; } }
        decimal w;
        public decimal WorkGroup { get { return w.Places(); } set { w = value; } }
        public decimal Total { get { return Reservation + Administration + Cooperation + Transcend + WorkGroup; } }
    }

}
