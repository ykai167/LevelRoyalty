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
    /// RoomCategoryControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.SuperAdminWindow, TabHeader = "房间类别", Order = 15)]
    public partial class RoomCategoryControl : UserControl
    {
        public RoomCategoryControl()
        {
            InitializeComponent();
        }
    }
}
