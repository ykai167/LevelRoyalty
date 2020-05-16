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
using System.Windows.Shapes;

namespace LR.WpfApp
{
    /// <summary>
    /// InitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InitWindow : Window
    {
        public InitWindow()
        {
            InitializeComponent();
            this.tbx.Text = "正在初始化, 请稍候......";
            this.Loaded += InitWindow_Loaded;
        }

        private async void InitWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var result = await Services.Initer.Init();
            this.tbx.Text = $"初始化完成,已生成超级管理员,请牢记用户名和密码!\r\n用户名:{result.Key}\r\n密码:{result.Value}";
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
