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
    /// RoomControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.MainWindow, TabHeader = "房间管理")]
    public partial class RoomControl : UserControl
    {
        LR.Services.IRoomService _service;

        public RoomControl(LR.Services.IRoomService service)
        {
            this._service = service;
            InitializeComponent();
            this.Loaded += RoomControl_Loaded;
        }

        private void RoomControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.dgData.DataContext = this._service.PageList(1);
        }

        private void txtEfficay_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
