using LR.Services;
using LR.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace LR.WpfApp.Models
{
    public class RoyaltySettleControlViewModel : Prism.Mvvm.BindableBase
    {
        IRoyaltySettleService _royaltySettleService;
        ISettleBatchService _settleBatchService;
        IRoyaltyService _royaltyService;

        public RoyaltySettleControlViewModel(
            IRoyaltySettleService royaltySettleService,
            ISettleBatchService settleBatchService,
            IRoyaltyService royaltyService)
        {
            this._royaltySettleService = royaltySettleService;
            this._settleBatchService = settleBatchService;
            this._royaltyService = royaltyService;
            this.batchs = settleBatchService.List().Where(p => p.IsHistory).OrderByDescending(item => item.CreateDate).Select(item => new SettleBatch
            {
                Num = item.Num,
                BeginEnd = $"{item.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}至{item.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}"
            }).ToArray();
            var num = this.batchs.FirstOrDefault()?.Num ?? 0;
            this.CurrentBatch = this.batchs.FirstOrDefault();
            this.Current = null;
            //this.BatchSelectedCommand = new Prism.Commands.DelegateCommand<object>(BatchSelected);
        }

        //Prism.Commands.DelegateCommand<object> BatchSelectedCommand;

        private SettleBatch[] batchs;

        public SettleBatch[] Batchs
        {
            get
            {
                return batchs;
            }
            set { batchs = value; }
        }
        SettleBatch currentBatch;
        public SettleBatch CurrentBatch
        {
            get
            {
                return currentBatch;
            }
            set
            {
                currentBatch = value;
                if (value != null)
                {
                    base.RaisePropertyChanged();
                    this.ChangeBatch(value.Num);
                }
            }
        }

        public List<RoyaltySettleExpendModel> Rows { get; private set; }

        private List<object> detailes;
        public List<object> Detailes
        {
            get { return detailes; }
            set
            {
                detailes = value;
                base.RaisePropertyChanged();
            }
        }

        public decimal ReservationTotal { get { return this.Rows == null ? 0 : this.Rows.Sum(p => p.Reservation).Places(); } }
        public decimal AdministrationTotal { get { return this.Rows == null ? 0 : this.Rows.Sum(p => p.Administration).Places(); } }
        public decimal CooperationTotal { get { return this.Rows == null ? 0 : this.Rows.Sum(p => p.Cooperation).Places(); } }
        public decimal TranscendTotal { get { return this.Rows == null ? 0 : this.Rows.Sum(p => p.Transcend).Places(); } }
        public decimal WorkGroupTotal { get { return this.Rows == null ? 0 : this.Rows.Sum(p => p.WorkGroup).Places(); } }
        public decimal AllTotal { get { return this.Rows == null ? 0 : this.Rows.Sum(p => p.Total).Places(); } }

        private RoyaltySettleExpendModel current;

        public RoyaltySettleExpendModel Current
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
                base.RaisePropertyChanged();
                base.RaisePropertyChanged(nameof(this.Enabled));
                base.RaisePropertyChanged(nameof(this.ShowReceiver));
                base.RaisePropertyChanged(nameof(this.ShowButton));
                base.RaisePropertyChanged(nameof(this.ShowChk));
                if (this.Current != null)
                {
                    this.Detailes = _royaltyService.Detaile(current.StaffID, num);
                }
                else
                {
                    this.Detailes = new List<object>();
                }
            }
        }
        int num;
        public void ChangeBatch(int num)
        {
            this.num = num;
            this.Rows = _royaltySettleService.GetSettles(num)
             .Select(item => new RoyaltySettleExpendModel
             {
                 ID = item.ID,
                 StaffNo = item.StaffNo,
                 StaffID = item.StaffID,
                 StaffName = item.StaffName,
                 Administration = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Administration).Value,
                 Cooperation = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Cooperation).Value,
                 Reservation = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Reservation).Value,
                 Transcend = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Transcend).Value,
                 WorkGroup = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.WorkGroup).Value,
                 IsExpend = item.IsExpend,
                 IsSelf = item.IsSelf,
                 Receiver = item.Receiver,
                 ExpendTime = item.ModifyDate,
                 Admin = item.Admin
             }).ToList();
            this.Current = null;
            base.RaisePropertyChanged(nameof(Rows));
            base.RaisePropertyChanged(nameof(AllTotal));
        }

        public Visibility ShowReceiver
        {
            get
            {
                if (current == null || current.IsSelf)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
        public bool Enabled
        {
            get
            {
                return !(Current?.IsExpend ?? true);
            }
        }
        public Visibility ShowButton
        {
            get
            {
                if (current == null || current.IsExpend)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        public Visibility ShowChk
        {
            get
            {
                return this.Current == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public void Expend(bool self, string name = null)
        {
            this._royaltySettleService.Update(this.Current.ID, new { IsSelf = self, Receiver = name, IsExpend = true });
            this.Current.IsSelf = self;
            this.Current.Receiver = name;
            this.Current.IsExpend = true;
            this.Current = this.Current;
            this.ChangeBatch(this.num);
            base.RaisePropertyChanged(nameof(this.Rows));
        }
    }

    public class SettleBatch
    {
        public int Num { get; set; }
        public string BeginEnd { get; set; }
    }

    public class RoyaltySettleExpendModel : RoyaltySettleModel
    {


        /// <summary>
        /// 是否已支出
        /// </summary>
        public bool IsExpend { get; set; }

        /// <summary>
        /// 是否本人领取
        /// </summary>
        public bool IsSelf { get; set; }

        /// <summary>
        /// 实际领取人
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// 发放时间
        /// </summary>
        public DateTime ExpendTime
        {
            get; set;
        }
        public string ExpandTimeStr { get { return IsExpend ? ExpendTime.ToString() : null; } }
        /// <summary>
        /// 是否本人
        /// </summary>
        public string Self { get { return IsSelf ? "是" : "否"; } }
        /// <summary>
        /// 是否已发
        /// </summary>
        public string Expend { get { return IsExpend ? "是" : ""; } }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Admin { get; set; }
    }
}
