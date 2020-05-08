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
    [UserControlUse(UseTo.MainWindow, TabHeader = "当前账期")]
    public partial class CurrentSettleControl : UserControl
    {
        public CurrentSettleControl()
        {
            InitializeComponent();
            this.DataContext = Tools.DIHelper.GetInstance<Models.CurrentSettleControlViewModel>();
            this.dgSettle.SelectionMode = DataGridSelectionMode.Single;
            this.dgSettle.SelectionChanged += DgSettle_SelectionChanged;
        }

        private void DgSettle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
