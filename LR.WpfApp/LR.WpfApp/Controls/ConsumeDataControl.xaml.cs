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
    /// ConsumeDataControl.xaml 的交互逻辑
    /// </summary>
    public partial class ConsumeDataControl : UserControl
    {
        LR.Services.IConsumeDataService _service;
        public ConsumeDataControl(LR.Services.IConsumeDataService service)
        {
            this._service = service;
            InitializeComponent();
            this.Loaded += ConsumeDataControl_Loaded;

            
        }

        private void ConsumeDataControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.dgData.DataContext = this._service.PageList(1);
        }
    }
}
