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

namespace LR.WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        TabSource[] sources = TabSource.GetTabSources(Controls.UseTo.MainWindow);

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            foreach (var item in sources)
            {
                var TabItem = new TabItem { Header = item.Header };
                this.tabMain.Items.Add(TabItem);
                this.tabMain.SelectionChanged += TabMain_SelectionChanged;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.tabMain.SelectionChanged += TabMain_SelectionChanged;
        }

        private void TabMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is TabControl)
            {
                (this.tabMain.Items[this.tabMain.SelectedIndex] as TabItem).Content = Tools.DIHelper.GetInstance(sources[this.tabMain.SelectedIndex].ControlType);
            }
            e.Handled = true;
        }
    }
}
