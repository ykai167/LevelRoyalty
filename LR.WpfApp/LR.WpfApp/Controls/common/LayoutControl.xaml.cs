using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// LayoutControl.xaml 的交互逻辑
    /// </summary>
    public partial class LayoutControl : UserControl
    {
        Models.LayOutViewModel vm;
        public LayoutControl()
        {
            InitializeComponent();
            this.Loaded += LayoutControl_Loaded;
        }

        private void LayoutControl_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new Models.LayOutViewModel()
            {
                AdminName = LR.Services.Administrator.Current.Name,
                DateTime = DateTime.Now
            };
            this.DataContext = vm;

        }

        public TabControl TabControl
        {
            get
            {
                return this.TabMain;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
