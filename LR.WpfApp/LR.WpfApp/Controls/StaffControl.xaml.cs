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
    /// StaffControl.xaml 的交互逻辑
    /// </summary>
    [UserControlUse(UseTo.MainWindow, TabHeader = "员工管理")]
    public partial class StaffControl : UserControl
    {
        LR.Services.IStaffService _service;

        public StaffControl(LR.Services.IStaffService service)
        {
            this._service = service;
            InitializeComponent();          
            this.Loaded += StaffControl_Loaded;
        }

        private void StaffControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.dgData.DataContext = this._service.PageList(1);
        }
    }
}
