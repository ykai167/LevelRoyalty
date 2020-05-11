﻿using LR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            //this.BatchSelectedCommand = new Prism.Commands.DelegateCommand<object>(BatchSelected);
        }

        //Prism.Commands.DelegateCommand<object> BatchSelectedCommand;

        private SettleBatch[] batchs;

        public SettleBatch[] Batchs
        {
            get { return batchs; }
            set { batchs = value; }
        }

        public List<RoyaltySettleExpendModel> Rows { get; private set; }


        private List<object> detailes;

        public List<object> Detailes
        {
            get { return detailes; }
            set { detailes = value; base.RaisePropertyChanged(); }
        }

        public void ChangeStaff(Guid staffID)
        {
            this.Detailes = _royaltyService.Detaile(staffID, num);
        }
        int num = 0;
        public void ChangeBatch(int num)
        {
            this.num = num;
            this.Rows = _royaltySettleService.GetSettles(num)
             .Select(item => new RoyaltySettleExpendModel
             {
                 StaffID = item.StaffID,
                 StaffName = MemoryData.Current.Staffs.FirstOrDefault(p => p.ID == item.StaffID)?.Name,
                 Administration = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Administration).Value,
                 Cooperation = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Cooperation).Value,
                 Reservation = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Reservation).Value,
                 Transcend = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.Transcend).Value,
                 WorkGroup = item.Items.FirstOrDefault(p => p.Key == RoyaltyType.WorkGroup).Value,
                 Total = item.Total,
                 IsExpend = item.IsExpend,
                 IsSelf = item.IsSelf,
                 Receiver = item.Receiver,
                 ExpendTime = item.ModifyDate

             }).ToList();
            this.Detailes = new List<object>();
            base.RaisePropertyChanged(nameof(Rows));
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

        public DateTime ExpendTime { get; set; }
    }
}
