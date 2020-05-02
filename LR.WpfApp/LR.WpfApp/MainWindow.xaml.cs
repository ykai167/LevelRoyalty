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
        /*<TabItem Header="员工">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="房间">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="工作组">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="消费记录">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="奖励数据">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="奖励发放">
                <Controls:AdminsControl />
            </TabItem>
            <TabItem Header="日志">
                <Controls:AdminsControl />
            </TabItem>*/
        static Models.TabSource[] Source = new[] {

             new Models.TabSource{  Header="员工", ControlType=typeof(Controls.AdminsControl)},
             new Models.TabSource{  Header="消费记录", ControlType=typeof(Controls.ConsumeDataControl)},
        };
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            foreach (var item in Source)
            {
                var TabItem = new TabItem { Header = item.Header, Content = Tools.DIHelper.GetInstance(item.ControlType) };
                this.tabMain.Items.Add(TabItem);
            }

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.tabMain.SelectionChanged += TabMain_SelectionChanged;
        }

        private void TabMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var aaa = (this.tabMain.Items[this.tabMain.SelectedIndex] as ContentControl).Content.;
        }
    }
}
