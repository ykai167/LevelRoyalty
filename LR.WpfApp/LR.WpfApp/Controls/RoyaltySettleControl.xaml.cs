using LR.Services;
using LR.Tools;
using LR.WpfApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LR.WpfApp.Controls
{
    /// <summary>
    /// RoyaltySettleControl.xaml 的交互逻辑
    /// </summary>    
    [UserControlUse(UseTo.MainWindow, TabHeader = "奖励发放", Order = 5)]
    public partial class RoyaltySettleControl : UserControl
    {
        RoyaltySettleControlViewModel vm;
        public RoyaltySettleControl()
        {
            InitializeComponent();
            this.DataContext = vm = Tools.DIHelper.GetInstance<Models.RoyaltySettleControlViewModel>();
            //this.cbx.SelectionChanged += Cbx_SelectionChanged;
        }

        private void chkSelf_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chkSelf.IsChecked.HasValue && this.vm.Current != null)
            {
                if (this.chkSelf.IsChecked.Value)
                {
                    this.vm.Current.IsSelf = true;
                    this.txtReceiver.Text = "";
                }
                else
                {
                    this.vm.Current.IsSelf = false;
                }
                this.vm.Current = this.vm.Current;
            }
        }

        private void btnConmit_Click(object sender, RoutedEventArgs e)
        {
            string name = this.txtReceiver.Text;
            if (!(this.chkSelf.IsChecked ?? false) && string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("未输入领取人");
                return;
            }
            this.vm.Expend(this.chkSelf.IsChecked ?? false, name);
        }

        //private void Cbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    vm.ChangeBatch(this.cbx.SelectedItem.GetObjectValue<int>("Num"));
        //}

        //private void LvwShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (sender == this.lvwShow && this.lvwShow.SelectedItem != null)
        //    {
        //        vm.ChangeStaff(this.lvwShow.SelectedItem.GetObjectValue<Guid>("StaffID"));
        //    }
        //}
    }
}
