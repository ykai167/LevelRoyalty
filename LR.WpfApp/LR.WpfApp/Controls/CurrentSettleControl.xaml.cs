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
    /// CurrentSettleControl.xaml 的交互逻辑
    /// </summary>    
    [UserControlUse(UseTo.MainWindow, TabHeader = "当前账期", Order = 3)]
    public partial class CurrentSettleControl : UserControl
    {
        CurrentSettleControlViewModel vm;
        public CurrentSettleControl()
        {
            InitializeComponent();
            this.DataContext = vm = Tools.DIHelper.GetInstance<Models.CurrentSettleControlViewModel>();
        }

        private void LvwShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == this.lvwShow && this.lvwShow.SelectedItem != null)
            {
                vm.ChangeStaff(this.lvwShow.SelectedItem.GetObjectValue<Guid>("StaffID"));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Tools.DIHelper.GetInstance<IRoyaltySettleService>().Settlement();
            vm.Reload();
        }
    }
}
