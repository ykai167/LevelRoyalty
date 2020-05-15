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
    /// LogsControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.SuperAdminWindow, TabHeader = "日志列表", Order = 30)]
    public partial class LogsControl : UserControl
    {
        LR.Services.ILogService _service;

        public LogsControl(LR.Services.ILogService service)
        {
            InitializeComponent();
            this._service = service;

            this.InitListView();
        }
        
        private void InitListView()
        {
            this.lvwShow.ItemsSource = this._service.GetAll();
        }        
    }
}
