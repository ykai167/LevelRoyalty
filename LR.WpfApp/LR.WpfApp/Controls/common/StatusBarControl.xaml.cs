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
    /// StatusBarControl.xaml 的交互逻辑
    /// </summary>
    public partial class StatusBarControl : UserControl
    {
        Models.LayOutViewModel vm = new Models.LayOutViewModel { };
        public StatusBarControl()
        {
            InitializeComponent();
            this.DataContext = vm;
            this.Loaded += StatusBarControl_Loaded;
        }

        private void StatusBarControl_Loaded(object sender, RoutedEventArgs e)
        {
            vm.AdminName = LR.Services.Administrator.Current?.Name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
